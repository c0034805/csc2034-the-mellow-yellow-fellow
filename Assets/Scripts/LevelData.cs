using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData{
    
    public int availableLevels;

    public LevelData(){
        availableLevels = SelectButtons.availableLevels;
    }
}
