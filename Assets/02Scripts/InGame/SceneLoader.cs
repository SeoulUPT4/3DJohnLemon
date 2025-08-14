using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneType
{
    Title,
    Lobby,
    InGame,
    InGame2
}

public class SceneLoader : MonoBehaviour
{
    public string nextSceneName;

    bool isMoving = false;

    public void LoadScene(string sceneName)
    {
        LoadingScreen.Instance.LoadScene(nextSceneName, 0.2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isMoving && other.transform == PlayerController.Instance.transform)
        {
            isMoving = true;
            LoadingScreen.Instance.LoadScene(nextSceneName, 0.2f);
        }
    }
}
