using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
public enum SlotType
{
    Inventory,
    Equipment,
    QuickSlot,
}
public class UIManager : MonoBehaviour
{
    //================== ½Ì±ÛÅæ==========================================
    public static UIManager Instance { get; private set; }

    void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }

    //====================================================================
    public Slider mouseSensivitySlider;
    public CinemachineFreeLook cinemachineFreeLook;


    [SerializeField] private InventoryUI m_InventoryUI;
    [SerializeField] private EquipmentUI m_EquipmentUI;
    [SerializeField] private StatsUI m_StatsUI;
    [SerializeField] private PlayerDataUI m_PlayerDataUI;
    [SerializeField] private QuestUI m_QuestUI;
    [SerializeField] private QuickSlotUIController m_QuickSlotUI;
    [SerializeField] private SkillUIController m_skillUIController;
    [SerializeField] private GameOverText m_gameOverText;
    [SerializeField] private bool m_btitle;
    [SerializeField] private bool m_bOpenPanel;
    private float x;
    private float y;


    public QuestUI _QuestUI => m_QuestUI;
    public StatsUI _StatsUI => m_StatsUI;
    public QuickSlotUIController _QuickSlotUI => m_QuickSlotUI;
    public InventoryUI _InventoryUI => m_InventoryUI;
    public SkillUIController _skillUIController => m_skillUIController;
    public PlayerDataUI _PlayerDataUI => m_PlayerDataUI;
    public EquipmentUI _EquipmentUI => m_EquipmentUI;
    public GameOverText _gameOverText => m_gameOverText;



    void Start()
    {
        if (m_btitle)
            return;
        cinemachineFreeLook.m_XAxis.m_MaxSpeed = mouseSensivitySlider.value * 300f;

        m_InventoryUI.Initialize();
        m_EquipmentUI.Initialize();
        m_StatsUI.Initalize();
        m_skillUIController.Initialize();
        //m_PlayerDataUI.Initialize();
    }

    // Update is called once per frame

    public void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
    }
    public void HidePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
    public void ChangeMouseSensivity()
    {
      cinemachineFreeLook.m_XAxis.m_MaxSpeed =100 +  mouseSensivitySlider.value* 200f;
    }
    public void SetCanAct(bool act)
    {
        PlayerController.Instance.SetCanAct(act);
    }
    public void UpdatePlayerData(string name, string job, string title, int Lv)
    {
        m_StatsUI.UpdatePlayerInfoUI(name,job,title,Lv);
    }
    public void QusetInfoSet(Quest data)
    {
        m_QuestUI.SetQuestInfoData(data);
    }
    public void QusetAnnounceUIUpdate()
    {

    }
    public void SkillUIUpdateAll()
    {
        PlayerController.Instance._PlayerSkill.UpdateSkillAll();
    }
    public void PlayBtnSFX()
    {
        AudioManager.Instance.PlaySFX(0);
    }
    public void LockUnLockMouseMove(bool b)
    {
        if(b)
        {
            if (cinemachineFreeLook.m_XAxis.m_MaxSpeed == 0 || cinemachineFreeLook.m_YAxis.m_MaxSpeed ==0)
            {
                return;
            }
            x = cinemachineFreeLook.m_XAxis.m_MaxSpeed;
            y = cinemachineFreeLook.m_YAxis.m_MaxSpeed;
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = 0;
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = 0;
        }
        else
        {
            Debug.Log("x " + x.ToString() + " y " + y.ToString());
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = x;
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = y;
        }
    }
} 
