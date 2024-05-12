using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance { get; private set; }
    void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }
    //=============================================================
    [SerializeField] private int [] m_StageMonsterCount;
    private int m_DungeonMonsterCount;
    private int m_curRemainMonsterCount;
    [SerializeField] private Animator m_DoorAnim;
    [SerializeField] private TextMeshProUGUI m_countText;
    [SerializeField] private Slider m_bossHealth;

    [SerializeField] private DungeonReward m_DungeonClear;
    [SerializeField] private PlayableDirector m_Clear;
    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        m_DungeonMonsterCount = 0;
        foreach (int count in m_StageMonsterCount)
        {
            m_DungeonMonsterCount += count;
        }
        m_curRemainMonsterCount = m_DungeonMonsterCount;
        UpdateMonsterCount();
        PlayerController.Instance.ResetPlayer();
    }
    public Slider GetHealthbar()
    {
        return m_bossHealth;
    }
    public void DieMonster()
    {
        m_curRemainMonsterCount--;
        UpdateMonsterCount();
    }
    public void UpdateMonsterCount()
    {
        string s = m_curRemainMonsterCount.ToString() + " / " + m_DungeonMonsterCount.ToString(); 
        m_countText.text = s;
        if(m_curRemainMonsterCount<=0)
        {
            OpenTheBossRoom();
        }
    }
    public void OpenTheBossRoom()
    {
        m_DoorAnim.SetBool("Open", true);
        AudioManager.Instance.PlaySFX(15);
        m_countText.text = "1 / 1";
    }
    public void DungeonClearOpen()
    {
        m_DungeonClear.gameObject.SetActive(true);
        int []rewards = new int[2];
        for(int i =0; i < Random.Range(0, 2);i++)
        {
            rewards[i] = Random.Range(13, 19);
        }
        m_DungeonClear.RewardSet(1000, 500, rewards);
    }
    public void DungeonClear()
    {

        StartCoroutine(DungeonClearDelay());
    }
    public IEnumerator DungeonClearDelay()
    {
        m_Clear.Play();
        PlayerController.Instance.SetCanAct(true);
        yield return new WaitForSeconds(8.2f);
        DungeonClearOpen();
    }
    public void GoVillage()
    {
        LoadingSceneManager.Instance.m_NextSceneName = "Village";
        SceneManager.LoadScene("Load");
    }
}

