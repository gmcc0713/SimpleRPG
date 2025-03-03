using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class GameMgr : MonoBehaviour
{
    public static GameMgr Instance { get; private set; }
    [SerializeField] PlayerInput playerInput;
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

    void Start()
    {
        BGMChange();
        SceneManager.sceneLoaded += OnSceneLoaded;
        Initialize();

    }
    public void Initialize()
    {
        playerInput.actions["RunMode"].performed += PlayerController.Instance.RunModeOn;                         //�޸��� ���(shift)
        playerInput.actions["RunMode"].canceled += PlayerController.Instance.RunModeOff;
        playerInput.actions["Move"].performed += PlayerController.Instance.OnMove;                               //wasd �����̱�
        playerInput.actions["Move"].canceled += PlayerController.Instance.OnMove;
        playerInput.actions["MouseLeftClick"].performed += PlayerController.Instance.OnMouseButtonDown;          //����(���콺����)
        playerInput.actions["UIOpenKey"].performed += PlayerController.Instance.UIOpenKey;          //����(���콺����)


        //playerInput.actions["InteractionButtonClick"].performed += ThirdPersonMovement.Instance.OnMouseButtonDown;          //��ȣ�ۿ�(space)
      
        ItemDataManager.Instance.Initialize();
        PlayerDataManager.Instance.Initialize();

        PlayerController.Instance.Initialize();
    }
    private void OnDisable()
    {   
        playerInput.actions["RunMode"].performed -= PlayerController.Instance.RunModeOn;
        playerInput.actions["RunMode"].canceled -= PlayerController.Instance.RunModeOff;
        playerInput.actions["Move"].performed -= PlayerController.Instance.OnMove;
        playerInput.actions["Move"].canceled -= PlayerController.Instance.OnMove;

        playerInput.actions["MouseLeftClick"].performed -= PlayerController.Instance.OnMouseButtonDown;
        playerInput.actions["UIOpenKey"].performed -= PlayerController.Instance.UIOpenKey;          //����(���콺����)

    }
    public void BGMChange()
    {
        int bgmNum = 0;

        switch (LoadingSceneManager.Instance.m_NextSceneName)
        {
            case "FirstDungeonMap":
                bgmNum = 2;
                break;
            case "Village":
                bgmNum = 1;
                break;
            case "TitleScene":
                bgmNum = 0;
                break;
        }
        AudioManager.Instance.ChangeBGM(bgmNum);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Load")
        {
            BGMChange();
            Initialize();
        }
        else
        {
            PlayerController.Instance.ResetPlayer();
        }

    }

}
