using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePicker : MonoBehaviour{

    public void LevelOne(){
        Manager.previousScene = SceneManager.GetActiveScene().name;
        Manager.Load(Manager.Scene.Level1Scene);
    }

    public void LevelTwo(){
        Manager.previousScene = SceneManager.GetActiveScene().name;
        Manager.Load(Manager.Scene.Level2Scene);
    }

    public void LevelThree(){
        Manager.previousScene = SceneManager.GetActiveScene().name;
        Manager.Load(Manager.Scene.Level3Scene);
    }

    public void LevelFour(){
        Manager.previousScene = SceneManager.GetActiveScene().name;
        Manager.Load(Manager.Scene.Level4Scene);
    }

    public void LevelFive(){
        Manager.previousScene = SceneManager.GetActiveScene().name;
        Manager.Load(Manager.Scene.Level5Scene);
    }

    public void Scoreboard(){
        Manager.previousScene = SceneManager.GetActiveScene().name;
        Manager.Load(Manager.Scene.ScoreboardScene);
    }

    public void MainMenu(){
        Fellow.totalScore = 0;
        Manager.previousScene = SceneManager.GetActiveScene().name;
        Manager.Load(Manager.Scene.MenuScene);
    }

    public void LevelSelect(string mode){
        GameUI.gameMode = mode;
        Manager.previousScene = SceneManager.GetActiveScene().name;
        Manager.Load(Manager.Scene.SelectScene);
    }

    public void GameModeSelect(){
        Manager.previousScene = SceneManager.GetActiveScene().name;
        Manager.Load(Manager.Scene.GameModeScene);
    }

    public void Exit(){
        Manager.previousScene = SceneManager.GetActiveScene().name;
        Debug.Log("Exited game successfully!");
        Application.Quit();
    }
}
