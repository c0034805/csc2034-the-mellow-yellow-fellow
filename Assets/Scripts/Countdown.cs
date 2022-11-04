using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Countdown : MonoBehaviour{

    [SerializeField]
    private float countdownLength = 3f;

    [SerializeField]
    private TextMeshProUGUI countdownText;

    private float countdown;

    private Ghost[] ghosts;

    private void Awake(){
        ghosts = (Ghost[]) FindObjectsOfType(typeof(Ghost));

        transform.gameObject.SetActive(false);
    }

    private void Update(){
        if(countdown > 0f){
            countdown -= Time.deltaTime;
        }
        else{
            countdown = 0f;
        }

        DisplayCountdown(countdown);
    }

    private void DisplayCountdown(float timeToDisplay){
        if(timeToDisplay < 0f){
            transform.gameObject.SetActive(false);
            GameObject.FindGameObjectsWithTag("Fellow")[0].GetComponent<Fellow>().SwitchControls();
        }
        else{
            foreach(Ghost ghost in ghosts){
                ghost.Freeze();
                ghost.ResetPosition();
                ghost.gameObject.SetActive(false);
                ghost.gameObject.SetActive(true);
            }
        }

        float seconds = Mathf.FloorToInt(timeToDisplay % 60) + 1f;

        countdownText.text = seconds.ToString();
    }

    public void ResetCountdown(){
        countdown = countdownLength;
    }
}
