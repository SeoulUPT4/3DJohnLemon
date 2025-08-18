using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Title,
    Lobby,
    InGame,
    InGame2
}

public class SceneLoader
{
    public void LoadScene(string sceneName)
    {
        LoadingScreen.Instance.LoadScene(sceneName, 0.2f);
    }
    public static void ReLoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
