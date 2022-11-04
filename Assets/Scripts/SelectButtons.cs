using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButtons : MonoBehaviour{
    
    [HideInInspector]
    public static int availableLevels = 1;

    [HideInInspector]
    public static int buttons = 0;

    private void Awake(){
        LoadAvailableLevels();
    }

    private void Start(){
        if(GameUI.gameMode == "hardcore"){
            return;
        }

        foreach(Transform child in transform){
            buttons++;
        }

        if(availableLevels > buttons){
            availableLevels = buttons;
        }

        for(int i = 0; i < availableLevels; i++){
            transform.Find("Level " + (i + 1).ToString() + " Button").GetComponent<Button>().interactable = true;
        }
    }

    public static void SaveAvailableLevels(){
        SaveLoadSystem.SaveLevels(availableLevels);
    }

    public void LoadAvailableLevels(){
        LevelData data = SaveLoadSystem.LoadLevels();
        if(data != null){
            availableLevels = data.availableLevels;
        }
    }
}
