using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fellow : MonoBehaviour{
    [SerializeField]
    Fellow playerObject;

    [SerializeField]
    public int lives = 3;

    [SerializeField]
    public float speed = 3f;

    [SerializeField]
    private int pointsPerPellet = 100;

    [SerializeField]
    private int pointsPerGhost = 200;

    [SerializeField]
    public float powerupDuration = 10.0f;

    [SerializeField]
    private Transform gameOverCanvas;

    [SerializeField]
    private Button retryButton;

    [SerializeField]
    private Button nextLevelButton;

    [HideInInspector]
    public float time = 0f;

    [HideInInspector]
    public int score = 0;

    [HideInInspector]
    public bool won = false;

    [HideInInspector]
    public bool lost = false;

    [HideInInspector]
    public bool inWall = false;

    [HideInInspector]
    public static int totalScore = 0;

    private float wallPowerupTime = 0.0f;
    private float scarePowerupTime = 0.0f;
    private float slowPowerupTime = 0.0f;
    private float scorePowerupTime = 0.0f;

    private Vector3 startLocation;

    private GameObject[] pellets;
    private int pelletsEaten = 0;
    private bool restorePoints = false;

    private bool flag = false;

    private bool controls = true;

    private bool keepCounting = true;

    private void Awake(){
        startLocation = playerObject.transform.position;
        pellets = GameObject.FindGameObjectsWithTag("Pellet");

        if(GameUI.gameMode == "hardcore"){
            lives = 1;
            score = totalScore;
        }

        ResetTime();

        Manager.currentScene = SceneManager.GetActiveScene().name;
    }

    private void Update(){
        if(playerObject.PelletsEaten() == pellets.Length && !flag){
            won = true;

            gameOverCanvas.gameObject.SetActive(true);
            gameOverCanvas.GetComponent<GameOver>().DisableButtons();
            flag = true;

            if(Int32.Parse(new String(SceneManager.GetActiveScene().name.Where(Char.IsDigit).ToArray())) == SelectButtons.availableLevels && GameUI.gameMode == "original"){
                if(!(SelectButtons.availableLevels == SelectButtons.buttons)){
                    SelectButtons.availableLevels++;
                }

                SelectButtons.SaveAvailableLevels();
            }

            keepCounting = false;

            if(GameUI.gameMode == "hardcore" && !(Int32.Parse(new String(SceneManager.GetActiveScene().name.Where(Char.IsDigit).ToArray())) == SelectButtons.buttons)){
                nextLevelButton.onClick.Invoke();
            }
        }
        
        if(GameObject.FindGameObjectsWithTag("Timer").Length < 1){
            if(keepCounting){
                time += Time.deltaTime;
            }

            scarePowerupTime = Mathf.Max(0.0f, scarePowerupTime - Time.deltaTime);
            wallPowerupTime = Mathf.Max(0.0f, wallPowerupTime - Time.deltaTime);
            slowPowerupTime = Mathf.Max(0.0f, slowPowerupTime - Time.deltaTime);
            scorePowerupTime = Mathf.Max(0.0f, scorePowerupTime - Time.deltaTime);
        }

        if(!ScorePowerupActive() && restorePoints){
            pointsPerPellet /= 2;
            restorePoints = !restorePoints;
        }
    }

    private void FixedUpdate(){
        if(controls){
            Rigidbody b = GetComponent<Rigidbody>();
            Vector3 velocity = b.velocity;

            if(Input.GetKey(KeyCode.A)){
                velocity.x = -speed;
            }
            if(Input.GetKey(KeyCode.D)){
                velocity.x = speed;
            }
            if(Input.GetKey(KeyCode.W)){
                velocity.z = speed;
            }
            if(Input.GetKey(KeyCode.S)){
                velocity.z = -speed;
            }

            b.velocity = velocity;
        }
    }

    public int PelletsEaten(){
        return pelletsEaten;
    }

    public bool ScarePowerupActive(){
        return scarePowerupTime > 0.0f;
    }

    public bool WallPowerupActive(){
        return wallPowerupTime > 0.0f;
    }

    public bool SlowPowerupActive(){
        return slowPowerupTime > 0.0f;
    }

    public bool ScorePowerupActive(){
        return scorePowerupTime > 0.0f;
    }

    public bool PowerupActive(){
        return ScarePowerupActive() || WallPowerupActive() || SlowPowerupActive() || ScorePowerupActive();
    }

    public void SwitchControls(){
        controls = !controls;
    }

    private void ResetTime(){
        time = 0f;
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Pellet")){
            pelletsEaten++;
            score += pointsPerPellet;
            totalScore = score;
        }

        if(other.gameObject.CompareTag("Powerup")){
            scarePowerupTime = powerupDuration;
        }

        if(other.gameObject.CompareTag("WallPowerup")){
            wallPowerupTime = powerupDuration;
        }

        if(other.gameObject.CompareTag("SlowPowerup")){
            slowPowerupTime = powerupDuration;
        }

        if(other.gameObject.CompareTag("ScorePowerup")){
            scorePowerupTime = powerupDuration;
            restorePoints = !restorePoints;
            pointsPerPellet *= 2;
        }

        if(other.transform.tag.Contains("Ghost")){
            if(other.transform.tag == "GhostMad" || (!ScarePowerupActive() && !other.transform.GetComponent<Ghost>().eaten)){
                if(lives > 1){
                    playerObject.gameObject.SetActive(false);
                    GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                    playerObject.transform.position = startLocation;
                    playerObject.gameObject.SetActive(true);

                    Resources.FindObjectsOfTypeAll<Countdown>()[0].gameObject.SetActive(true);
                    Resources.FindObjectsOfTypeAll<Countdown>()[0].ResetCountdown();
                    playerObject.SwitchControls();
                }
                else{
                    if(GameUI.gameMode == "original" || GameUI.gameMode == "hardcore"){
                        lost = true;
                        gameOverCanvas.gameObject.SetActive(true);
                        gameOverCanvas.GetComponent<GameOver>().DisableButtons();

                        playerObject.gameObject.SetActive(false);

                        if(GameUI.gameMode == "hardcore"){
                            retryButton.onClick.RemoveAllListeners();
                            retryButton.onClick.AddListener( () => SceneManager.LoadScene("Level1Scene"));
                        }
                    }
                    else{
                        retryButton.onClick.Invoke();
                    }
                }
                lives--;
            }
            else{
                if(!other.gameObject.GetComponent<Ghost>().eaten){
                    other.gameObject.GetComponent<Ghost>().eaten = true;
                    score += pointsPerGhost;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.CompareTag("Wall")){
            inWall = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Wall")){
            inWall = false;
        }
    }
}
