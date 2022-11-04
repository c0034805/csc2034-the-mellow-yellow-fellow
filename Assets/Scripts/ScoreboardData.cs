using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreboardData{

    public string[] names;
    public int[] scores;
    public float[] times;

    public ScoreboardData(){
        names = new string[Scoreboard.allScores.Count];
        scores = new int[Scoreboard.allScores.Count];
        times = new float[Scoreboard.allScores.Count];

        if(Scoreboard.allScores.Count > 0){
            for(int i = 0; i < Scoreboard.allScores.Count; i++){
                names[i] = Scoreboard.allScores[i].name;
                scores[i] = Scoreboard.allScores[i].score;
                times[i] = Scoreboard.allScores[i].time;
            }
        }
    }
}
