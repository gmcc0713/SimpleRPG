using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverText : MonoBehaviour
{
    [SerializeField] private Image Panel;
    float currentTime = 0;
    float fadeoutTime = 2;
    [SerializeField] private Button[] m_gameOverBtns;
    public void OnEnable()
    {
        FadeOutPlay();
    }
    public void FadeOutPlay()
    {
        StartCoroutine(fadeOut());
    }
    IEnumerator fadeOut()
    {
        Panel.gameObject.SetActive(true);
        Color alpha = Panel.color;
        while (alpha.a < 1)
        {
            currentTime += Time.deltaTime / fadeoutTime;
            alpha.a = Mathf.Lerp(0, 1, currentTime);
            Panel.color = alpha;
            yield return null;
        }
        m_gameOverBtns[0].gameObject.SetActive(true);
        m_gameOverBtns[1].gameObject.SetActive(true);
        //Time.timeScale = 0;

    }
    public void GameRestart()
    {
        LoadingSceneManager.Instance.m_NextSceneName = "Village";
        SceneManager.LoadScene("Load");
        ResetData();
        PlayerController.Instance.Respawn();
    }
    public void GameExit()
    {
        ResetData();
        Application.Quit();
    }
    public void ResetData()
    {
        m_gameOverBtns[0].gameObject.SetActive(false);
        m_gameOverBtns[1].gameObject.SetActive(false);
        gameObject.SetActive(false);
        currentTime = 0;
    }
}
