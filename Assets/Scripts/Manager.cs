using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Manager{

    private class LoadingMonoBehaviour : MonoBehaviour{}

    public enum Scene{
        MenuScene,
        GameModeScene,
        SelectScene,
        ScoreboardScene,
        LoadingScene,
        Level1Scene,
        Level2Scene,
        Level3Scene,
        Level4Scene,
        Level5Scene
    }

    public static string previousScene;
    public static string currentScene;

    private static Action onLoaderCallback;
    private static AsyncOperation asyncOperation;

    public static void Load(Scene scene){
        if(scene.ToString().Contains("Level")){
            onLoaderCallback = () => {
                GameObject loadingGameObject = new GameObject("Loading Game Object");
                loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
            };

            SceneManager.LoadScene(Scene.LoadingScene.ToString());
        }
        else{
            SceneManager.LoadScene(scene.ToString());
        }
    }

    public static void LoaderCallback(){
        if(onLoaderCallback != null){
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }

    public static float LoadingProgress(){
        if(asyncOperation != null){
            return asyncOperation.progress;
        }
        return 0f;
    }

    private static IEnumerator LoadSceneAsync(Scene scene){
        yield return null;
        asyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
        while(!asyncOperation.isDone){
            yield return null;
        }
    }
}
