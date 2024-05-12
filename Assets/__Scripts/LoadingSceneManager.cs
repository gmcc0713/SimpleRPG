using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSceneManager : MonoBehaviour
{
    public static LoadingSceneManager Instance { get; private set; }
    public string m_NextSceneName;

    void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            return;
        }
        Destroy(gameObject);
    }
}
