
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUI : MonoBehaviour{

    [SerializeField]
    private Fellow player;

    [HideInInspector]
    public static string gameMode = "original";

    private Transform ui;

    private Transform uiTransform;

    private void Awake(){
        ui = transform.Find("UITemplate");
        ui.gameObject.SetActive(false);

        uiTransform = Instantiate(ui, transform);
        uiTransform.gameObject.SetActive(true);

        int currentLevel = Int32.Parse(new String(SceneManager.GetActiveScene().name.Where(Char.IsDigit).ToArray()));

        Scoreboard.LoadHighScoreTable(currentLevel, gameMode);
        Scoreboard.SortHighScoreTable();
        
        if(Scoreboard.allScores.Count != 0){
            if(gameMode != "timed"){
                uiTransform.Find("HighScore").GetComponent<TextMeshProUGUI>().text = "Current High Score: " + Scoreboard.allScores[0].score;
            }
            else{
                int minutes = Mathf.FloorToInt(Scoreboard.allScores[0].time / 60);
                int seconds = Mathf.FloorToInt(Scoreboard.allScores[0].time % 60);

                uiTransform.Find("HighScore").GetComponent<TextMeshProUGUI>().text = "Current Best Time: " + minutes.ToString() + ":" + seconds.ToString("00");
            }
        }
        else{
            if(gameMode != "timed"){
                uiTransform.Find("HighScore").GetComponent<TextMeshProUGUI>().text = "Current High Score: 0";
            }
            else{
                uiTransform.Find("HighScore").GetComponent<TextMeshProUGUI>().text = "Current Best Time: 0:00";
            }
        }

        LoadUI();
    }

    private void Update(){
        LoadUI();
    }

    private void LoadUI(){
        uiTransform.Find("Lives").GetComponent<TextMeshProUGUI>().text = "Lives: " + player.lives;

        if(gameMode == "timed"){
            int minutes = Mathf.FloorToInt(player.time / 60);
            int seconds = Mathf.FloorToInt(player.time % 60);

            uiTransform.Find("Score").GetComponent<TextMeshProUGUI>().text = "Time: " + minutes.ToString() + ":" + seconds.ToString("00");
        }
        else{
            uiTransform.Find("Score").GetComponent<TextMeshProUGUI>().text = "Score: " + player.score;
        }
    }
}
