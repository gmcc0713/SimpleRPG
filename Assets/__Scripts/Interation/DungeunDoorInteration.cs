using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeunDoorInteration : InteractionObject
{
    public override void PressInteractionKey()
    {
        LoadingSceneManager.Instance.m_NextSceneName = "FirstDungeonMap";
        SceneManager.LoadScene("Load");
    }
}
