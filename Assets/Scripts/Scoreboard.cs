using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Scoreboard : MonoBehaviour{

    public struct HighScoreEntry{
        public string name;
        public int score;
        public float time;
    }

    public static List<HighScoreEntry> allScores = new List<HighScoreEntry>();

    private static Transform entries;
    private static Transform template;
    private static Transform buttons;
    private static TextMeshProUGUI scoreHeader;

    private static int pages;
    private static int currentPage = 1;

    private void Awake(){
        Manager.currentScene = SceneManager.GetActiveScene().name;
        
        LoadHighScoreTable(1, "original");
        SortHighScoreTable();

        FindChildren();

        FindPages();

        template.gameObject.SetActive(false);

        DeletePage();
        LoadPage();
    }

    private void FindChildren(){
        entries = transform.Find("Score Entries");
        template = entries.Find("Score Template");
        buttons = transform.Find("Navigation Buttons");
        scoreHeader = transform.Find("Headers").Find("Score Header").GetComponent<TextMeshProUGUI>();
    }

    public static void FindPages(){
        if(allScores.Count == 0){
            pages = 1;
        }
        else{ 
            pages = (int) Math.Ceiling((float) allScores.Count / 10);
        }
        
        buttons.Find("Previous Page Button").GetComponent<Button>().gameObject.SetActive(false);

        if(pages < 2){
            buttons.Find("Next Page Button").GetComponent<Button>().gameObject.SetActive(false);
        }
        else{
            buttons.Find("Next Page Button").GetComponent<Button>().gameObject.SetActive(true);
        }
    }

    public static void LoadPage(){
        if(GameUI.gameMode == "timed"){
            scoreHeader.text = "Time";
        }
        else{
            scoreHeader.text = "Score";
        }

        float templateHeight = 24f;

        if(allScores.Count == 0){
            Transform entryTransform = Instantiate(template, entries);
            entryTransform.gameObject.SetActive(true);
            entryTransform.Find("Position").gameObject.SetActive(false);
            entryTransform.Find("Score").gameObject.SetActive(false);

            entryTransform.gameObject.tag = "CloneEntry";

            entryTransform.Find("Name").GetComponent<TextMeshProUGUI>().text = "This level hasn't been completed yet";
            entryTransform.Find("Name").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -templateHeight);
            entryTransform.Find("Name").GetComponent<RectTransform>().sizeDelta = new Vector2(300f, 20f);

            return;
        }

        int n = (allScores.Count - (currentPage - 1) * 10);

        if(n > 10){
            n = 10;
        }
        
        for(int i = (currentPage - 1) * 10; i < (currentPage - 1) * 10 + n; i++){
            Transform entryTransform = Instantiate(template, entries);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * (i % 10));
            entryTransform.gameObject.SetActive(true);

            entryTransform.gameObject.tag = "CloneEntry";

            int pos = i + 1;
            entryTransform.Find("Position").GetComponent<TextMeshProUGUI>().text = pos.ToString();
            entryTransform.Find("Name").GetComponent<TextMeshProUGUI>().text = allScores[i].name;

            if(GameUI.gameMode == "timed"){
                int minutes = Mathf.FloorToInt(allScores[i].time / 60);
                int seconds = Mathf.FloorToInt(allScores[i].time % 60);

                entryTransform.Find("Score").GetComponent<TextMeshProUGUI>().text = minutes.ToString() + ":" + seconds.ToString("00");
            }
            else{
                entryTransform.Find("Score").GetComponent<TextMeshProUGUI>().text = allScores[i].score.ToString();
            }
        }
    }

    public static void DeletePage(){
        GameObject[] toDelete = GameObject.FindGameObjectsWithTag("CloneEntry");

        foreach(GameObject entry in toDelete){
            Destroy(entry);
        }
    }

    public void NextButton(){
        DeletePage();

        currentPage++;
        LoadPage();

        buttons.Find("Previous Page Button").GetComponent<Button>().gameObject.SetActive(true);

        if(currentPage == pages){
            buttons.Find("Next Page Button").GetComponent<Button>().gameObject.SetActive(false);
        }
    }

    public void PreviousButton(){
        DeletePage();

        currentPage--;
        LoadPage();

        buttons.Find("Next Page Button").GetComponent<Button>().gameObject.SetActive(true);

        if(currentPage == 1){
            buttons.Find("Previous Page Button").GetComponent<Button>().gameObject.SetActive(false);
        }
    }

    public static void LoadHighScoreTable(int level, string mode){
        allScores.Clear();

        ScoreboardData data = SaveLoadSystem.LoadScores(level, mode);

        if(data != null && data.scores != null){
            for(int i = 0; i < data.scores.Length; i++){
                HighScoreEntry entry;
                entry.name = data.names[i];
                entry.score = data.scores[i];
                entry.time = data.times[i];
                allScores.Add(entry);
            }
        }
    }

    public static void SaveHighScoreTable(int level, string mode){
        SaveLoadSystem.SaveScores(level, mode);
    }

    public static void AddEntry(HighScoreEntry entry){
        int currentLevel = Int32.Parse(new String(SceneManager.GetActiveScene().name.Where(Char.IsDigit).ToArray()));
        
        LoadHighScoreTable(currentLevel, GameUI.gameMode);
        allScores.Add(entry);
        SortHighScoreTable();

        SaveHighScoreTable(currentLevel,GameUI.gameMode);
    }

    public static void SortHighScoreTable(){
        if(GameUI.gameMode != "timed"){
            allScores.Sort((x, y) => y.score.CompareTo(x.score));
        }
        else{
            allScores.Sort((x, y) => x.time.CompareTo(y.time));
        }
    }
}
