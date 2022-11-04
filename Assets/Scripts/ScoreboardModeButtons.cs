using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardModeButtons : MonoBehaviour
{
    
    public void ScoreModeButton(string mode){
        GameUI.gameMode = mode;

        Scoreboard.DeletePage();
        Scoreboard.LoadHighScoreTable(Int32.Parse(new String(ScoreboardLevelButtons.selectedButton.name.Where(Char.IsDigit).ToArray())), mode);
        Scoreboard.SortHighScoreTable();
        Scoreboard.FindPages();
        Scoreboard.LoadPage();
    }
}
