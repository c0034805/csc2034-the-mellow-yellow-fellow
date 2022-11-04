using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScoreboardLevelButtons : MonoBehaviour{

    [HideInInspector]
    public static Button selectedButton;

    private string levelNumber = "1";

    private void Start(){
        if(Manager.previousScene.Contains("Level")){
            levelNumber = new String(Manager.previousScene.Where(Char.IsDigit).ToArray());
        }

        selectedButton = transform.Find("Level " + levelNumber + " Score Button").GetComponent<Button>();
        
        selectedButton.Select();
        selectedButton.onClick.Invoke();
    }

    private void Update() {
        selectedButton.Select();

        if(GameUI.gameMode == "hardcore"){
            foreach(Transform child in transform){
                child.gameObject.SetActive(false);
            }
        }
        else{
            foreach(Transform child in transform){
                child.gameObject.SetActive(true);
            }
        }
    }

    public void LevelScoreButton(int levelNumber){
        selectedButton = transform.Find("Level " + levelNumber + " Score Button").GetComponent<Button>();
        Scoreboard.DeletePage();
        Scoreboard.LoadHighScoreTable(levelNumber, GameUI.gameMode);
        Scoreboard.SortHighScoreTable();
        Scoreboard.FindPages();
        Scoreboard.LoadPage();
    }
}
