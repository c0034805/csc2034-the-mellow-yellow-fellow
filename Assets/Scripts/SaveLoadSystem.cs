using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoadSystem{
    
    public static void SaveLevels(int availableLevels){
        BinaryFormatter formatter = new BinaryFormatter();
        
        string path = FilePathLevels();
        FileStream stream = new FileStream(path, FileMode.Create);

        LevelData data = new LevelData();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static LevelData LoadLevels(){
        string path = FilePathLevels();

        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            LevelData data = null;

            if(stream.Length != 0){
                data = formatter.Deserialize(stream) as LevelData;
            }

            stream.Close();

            return data;
        }
        else{
            return null;
        }
    }

    public static void SaveScores(int level, string mode){
        BinaryFormatter formatter = new BinaryFormatter();
        
        string path = FilePathScores(level, mode);
        FileStream stream = new FileStream(path, FileMode.Create);

        ScoreboardData data = new ScoreboardData();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static ScoreboardData LoadScores(int level, string mode){
        string path = FilePathScores(level, mode);

        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            ScoreboardData data = null;

            if(stream.Length != 0){
                data = formatter.Deserialize(stream) as ScoreboardData;
            }

            stream.Close();

            return data;
        }
        else{
            return null;
        }
    }

    private static string FilePathLevels(){
        return Application.persistentDataPath + "/levels.myf";
    }

    private static string FilePathScores(int level, string mode){
        if(mode == "hardcore"){
            return Application.persistentDataPath + "/HardcoreScores.ymf";
        }
        
        return Application.persistentDataPath + "/Level" + level.ToString() + "SceneScores" + mode + ".myf";
    }
}
