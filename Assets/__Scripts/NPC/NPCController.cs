using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private List<int> m_iNPCQuestID;
    [SerializeField] private NPCData data;
    [SerializeField] private NPCInteractionUI npcUI;

    [SerializeField] private int curQuestIndex;  
    //public List<Quest_Data> quests;

    [SerializeField] private List<Quest_Data> quests;
    public List<Quest_Data> _quests => quests;

    private void Start()
    {
    }
    public int GetQuestId()
    {
        Debug.Log(curQuestIndex);
        return quests[curQuestIndex].m_iQuestID;
    }
    public void QuestUpdate()
    {
        if(quests[curQuestIndex].isClear && quests.Count> curQuestIndex-1)
        {
            curQuestIndex++;
        }
    }
    public void AcceptQuest()
    {
        //�÷��̾� ������ ����Ʈ �䱸���� ���� ���� �ȵǰ� ����
        if(!quests[curQuestIndex].isClear)
        {
            QuestManager.Instance.QuestAdd(quests[curQuestIndex].m_iQuestID);  //����Ʈ ���̵�� ����Ʈ ����
        }
       
    }
    public void AddQuest(Quest_Data data)
    {
        quests.Add(data);
    }
    public Quest_Data GetQuestData()
    {
        return _quests[curQuestIndex];
    }
    public void initialize()
    {
        quests = new List<Quest_Data>();
        if (m_iNPCQuestID == null)
            return;
        for (int i = 0; i < m_iNPCQuestID.Count; i++)
        {
            quests.Add(QuestManager.Instance.GetQuestData(m_iNPCQuestID[i]));
        }
        curQuestIndex = 0;
    }
    //�÷��̾ ������ ���� ����
    //�ش� �浹 �ϴ� �ױ� Ȯ�� �� Ư�� �ൿ ����


    public void UseInterationKey()
    {
        npcUI.UpdateUI(data);
        UIManager.Instance.ShowPanel(dialogPanel);
        PlayerController.Instance.SetCanAct(false);
        UIManager.Instance.LockUnLockMouseMove(true);
    }

}
