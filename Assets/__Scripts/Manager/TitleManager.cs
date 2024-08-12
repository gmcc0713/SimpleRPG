using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public  void GamePlay()
    {
        LoadingSceneManager.Instance.m_NextSceneName = "Village";
        SceneManager.LoadScene("Load");
    }
    public void GameExit()
    {
        Application.Quit();
    }
}
