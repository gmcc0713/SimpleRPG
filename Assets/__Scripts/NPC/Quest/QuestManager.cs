using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//����Ʈ UI�� ����ü or Ŭ����(�����ͺ���� ����),
public enum Quest_State
{
    Begin,
    Process,
    Clear,
}
public enum Announcement_Type
{
    Accept = 0,
    Refuse,
    Non,
}
[System.Serializable]
public struct Quest_Data
{
    public int m_iNPCID;
    public int m_iQuestID;
    public string m_sTitle;
    public string m_Description;
    public Quest_Type m_eType;
    public List<string> m_lDialog;
    public Quest_Reward m_sReward;
    public Require_Quest m_sRequire;

    public string[] m_arrAnnouncement;

    public bool isClear;
    
}
[System.Serializable]
public struct Quest_Reward
{
    public int rewardGold;
    public int rewardXp;
    public Dictionary<int, int> rewardItem;    //������ id,����
}
[System.Serializable]
public struct Require_Quest
{
    public int id;
    public int count;
    public int requireLV;
}
public enum Quest_Type
{
    Type_Kill = 0,
    Type_Collect,
    Type_Talk,
}
public class QuestManager : MonoBehaviour
{
    //=============================================================
    public static QuestManager Instance { get; private set; }
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
    //=============================================================
    [SerializeField] private List<Quest> m_lProgressQuests;
    [SerializeField] private List<Quest> m_Quests;
    [SerializeField] private QuestLoader m_questLoader;

    void Start()
    {
        m_lProgressQuests = new List<Quest>();
        m_Quests = new List<Quest>();
        m_questLoader.RewardLoad(); // ����Ʈ ���� �ҷ�����
        m_questLoader.LoadQuests();
        List<Quest_Data> questDatas = m_questLoader.GetQuestDatas();
        foreach(Quest_Data data in questDatas)
        {

            AddQuestInQuestList(data);

        }
    }
    public Quest GetQuestByIdx(int idx)
    {
        return m_lProgressQuests[idx];
    }
    public Quest_Data GetQuestDataByIdx(int idx)
    {
        return m_lProgressQuests[idx]._sData;
    }
    private void AddQuestInQuestList(Quest_Data data)
    {
        Quest q;
        switch (data.m_eType)
        {
            case Quest_Type.Type_Kill:
                q = new KillQuest();
                break;
            case Quest_Type.Type_Collect:
                q = new CollectQuest();
                break;
            default:
                q = new Quest();
                break;
        }
        q.SetData(data);
        m_Quests.Add(q);
    }

    public Quest_Data GetQuestData(int questID)
    {
        return m_Quests[questID]._sData;
    }

    public void NotifyQuestUpdate(Quest_Type type,int id, int amount)
    {
        if (m_lProgressQuests.Count == 0)
        {
            return;
        }
        foreach(var quest in m_lProgressQuests)
        {
            quest.UpdateQuest(type, id, amount);
        }
    }

    public bool CheckAlreadyClearQuest(int id)
    {
        foreach (var quest in m_lProgressQuests)
        {
            if(quest.QuestIDCheck(id))
            {
                if(!quest.IsClearQuest())
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void QuestAddByNPCID(int npcType)
    {
        QuestAdd(NPCManager.Instance.GetQuest(npcType).m_iQuestID);
    }
    public void QuestAdd(int id)
    {
        m_lProgressQuests.Add(m_Quests[id]);    //�������� ����Ʈ ��Ͽ� �߰�

        UIManager.Instance._QuestUI.QuestAddAndMakeQuestNode(m_lProgressQuests.Count-1);
    }
    
    public void ClearQuest(int idx)
    {
        Quest_Reward reward = m_lProgressQuests[idx]._sData.m_sReward;
        m_lProgressQuests[idx].ClearQuest();
        PlayerController.Instance._Inventory.AddGold(reward.rewardGold);

        //foreach (var rw in reward.rewardItem)
        {
            //Inventory.Instance.GetItem(rw.Key, rw.Value);
        }
        UIManager.Instance._QuestUI.RemoveQuest();
    }
    public void LoadQuset(List<Quest> progressQuests)
    {

        m_lProgressQuests = progressQuests;
    }
}
