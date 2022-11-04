using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour{

    [SerializeField]
    private Fellow player;

    private bool flag = false;

    private void Awake() {
       transform.gameObject.SetActive(false);
    }

    private void Update(){
        if(!flag && (player.won || player.lost)){
            GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");

            foreach(GameObject ghost in ghosts){
                ghost.transform.GetComponent<Ghost>().Freeze();
            }

            SetGameOver();
            flag = true;
        }
    }

    public void SubmitScore(){
        string text = transform.Find("Game Over").Find("InputField").GetComponent<TMP_InputField>().text;

        if(!string.IsNullOrWhiteSpace(text)){
            Scoreboard.HighScoreEntry entry;
            entry.name = text;
            entry.score = player.score;
            entry.time = player.time;

            Scoreboard.AddEntry(entry);

            transform.Find("Game Over").Find("Submit Button").GetComponent<Button>().gameObject.SetActive(false);
            transform.Find("Game Over").Find("InputField").GetComponent<TMP_InputField>().interactable = false;
            EnableButtons();
        }
    }

    public void DisableButtons(){
        transform.Find("Game Over").Find("Main Menu Button").GetComponent<Button>().gameObject.SetActive(false);
        transform.Find("Game Over").Find("Scoreboard Button").GetComponent<Button>().gameObject.SetActive(false);
        transform.Find("Game Over").Find("Next Level Button").GetComponent<Button>().gameObject.SetActive(false);
        transform.Find("Game Over").Find("Retry Button").GetComponent<Button>().gameObject.SetActive(false);
    }

    private void EnableButtons(){
        transform.Find("Game Over").Find("Main Menu Button").GetComponent<Button>().gameObject.SetActive(true);
        transform.Find("Game Over").Find("Scoreboard Button").GetComponent<Button>().gameObject.SetActive(true);

        if(player.won){
            if(Int32.Parse(new String(SceneManager.GetActiveScene().name.Where(Char.IsDigit).ToArray())) == SelectButtons.availableLevels){
                RectTransform scoreboard = transform.Find("Game Over").Find("Scoreboard Button").GetComponent<RectTransform>();
                RectTransform nextLevel = transform.Find("Game Over").Find("Next Level Button").GetComponent<RectTransform>();
                
                scoreboard.position = new Vector3(nextLevel.position.x, scoreboard.position.y, scoreboard.position.z);
            }
            else{
                transform.Find("Game Over").Find("Next Level Button").GetComponent<Button>().gameObject.SetActive(true);
            }
        }
        else if(player.lost){
            transform.Find("Game Over").Find("Retry Button").GetComponent<Button>().gameObject.SetActive(true);
        }
    }

    private void SetGameOver(){
        if(player.won){
            transform.Find("Game Over").Find("Result").GetComponent<TextMeshProUGUI>().text = "LEVEL COMPLETED!";
            transform.Find("Game Over").Find("Result").GetComponent<TextMeshProUGUI>().color = Color.green;
        }
        else if(player.lost){
            transform.Find("Game Over").Find("Result").GetComponent<TextMeshProUGUI>().text = "YOU LOSE";
            transform.Find("Game Over").Find("Result").GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        transform.Find("Game Over").Find("Final Score").GetComponent<TextMeshProUGUI>().text = "FINAL SCORE: " + player.score;
    }
}