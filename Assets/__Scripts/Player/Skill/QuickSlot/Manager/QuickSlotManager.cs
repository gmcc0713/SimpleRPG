using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum QuickSlotType
{
    Skill,
    Item
}

public class QuickSlotManager : MonoBehaviour
{
    public static QuickSlotManager Instance { get; private set; }
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
    //==================================================================================

    private ItemQuickSlot[] m_quickSlotItems;
    [SerializeField] private string[] m_quickSlotKeyCode;

    [SerializeField] private SkillQuickSlot[] m_quickSlotSkills;


    [SerializeField] private string[] m_quickSlotSkillKeyCode;
    private int m_iSkillEquipmentID; //��ų ������ ������ ���̵�
    public void SetSkillEquipmentID(int id) { m_iSkillEquipmentID = id; }
    public void UpdateAllUI()
    {
        if(m_quickSlotItems != null && m_quickSlotSkills != null)
        {
            for (int i = 0; i < m_quickSlotItems.Length; i++)
            {
                if (!m_quickSlotItems[i]._bIsEmpty)
                    m_quickSlotItems[i].UpdateItemSlotAddItem(i);
            }
            for (int i = 0; i < m_quickSlotSkills.Length; i++)
            {
                if (!m_quickSlotSkills[i]._bIsEmpty)
                {
                    m_quickSlotSkills[i].UpdateUI();
                }
            }
        }
    }
    public void Initialize()
    {
        Debug.Log("InitQuick");
        m_quickSlotItems = new ItemQuickSlot[4];
        for (int i = 0; i < m_quickSlotItems.Length; i++)
        {
            m_quickSlotItems[i] = new ItemQuickSlot();
            m_quickSlotItems[i].Initialize(i, m_quickSlotKeyCode[i]);
        }

        m_quickSlotSkills = new SkillQuickSlot[4];
        for (int i = 0; i < m_quickSlotSkills.Length; i++)
        {
            m_quickSlotSkills[i] = new SkillQuickSlot();
            m_quickSlotSkills[i].Initialize(i, m_quickSlotSkillKeyCode[i]);
        }
    }

    public void SkillQuickSlotEquipment()
    {
            foreach (SkillQuickSlot slot in m_quickSlotSkills)
            {
                UIManager.Instance._QuickSlotUI.SkillSlotSetColor(slot._iSlotNum, !slot._bIsEmpty && slot._ID == m_iSkillEquipmentID);

            }
    }



    public void ItemAddQuickSlotUpdate(int itemId) //�Է¹޴� id ���� �������� �߰��Ǿ �ش� �������� ���� �Ǿ����� Ȯ�� �� ������Ʈ
    {

        foreach (ItemQuickSlot slot in m_quickSlotItems)
        {
            if (slot.IsItemIDSame(itemId))
            {
                slot.UpdateItemInLinkedIvslot();
                break;
            }
        }
    }
    
    public bool QuickSlotHasSkillID(int skillId)
    {
        foreach(SkillQuickSlot slot in m_quickSlotSkills)
        { 
            if(skillId == slot._ID)
            {
                return true;
            }
        }
        return false;
    }
    public void SetItemInQuickSlot(int quickSlotNum, int ivSlotNum) //�������� ��ȣ�� �޾� �ش� ��ȣ�� �����Կ� ������ ����
    {
        m_quickSlotItems[quickSlotNum].UpdateItemSlotAddItem(ivSlotNum);
    }
    public void UpdateQuickSlotByQuickSlotNum(int quickSlotNum) //�������� ��ȣ�� �Է¹����� �ش� ��ȣ�� �����Ը� ������Ʈ �ȴ�.
    {
        m_quickSlotItems[quickSlotNum].UpdateItemInLinkedIvslot();
    }

    public void SetSkillDataInQuickSlot(int quickSlotNum, int skillID)
    {
        m_quickSlotSkills[quickSlotNum].SetSkillDataInQuickSlot(skillID);

    }

    private void Update()
    {
        if (m_quickSlotSkills[0] == null)
            return;
        foreach (ItemQuickSlot slot in m_quickSlotItems)
        {
            if ( !slot._bIsEmpty)
            {
                slot.IsPressedKey();
            }

        }
        foreach (SkillQuickSlot slot in m_quickSlotSkills)
        {
            if (slot._ID != -1&&!slot._bIsEmpty)
            {
                slot.IsPressedKey();
            }

        }
    }
    public void UseItem(int itemNum)
    {
        foreach(ItemQuickSlot slot in m_quickSlotItems)
        {
           if(slot.IsItemIDSame(itemNum))
            {
                slot.UpdateItemInLinkedIvslot();
            }
        }
    }
    public void CoolTimeStart(QuickSlot slot) //��Ÿ�� ����
    {
        slot.StartTimer();
        StartCoroutine(Cooltime(slot));
        StartCoroutine(CoolDown(slot));
    }
    public IEnumerator Cooltime(QuickSlot slot)
    {

        while (slot.fillAmountValue > 0)
        {
            slot.CoolTimeRunFillUIUpdate();
            slot.fillAmountValue -= 1 * Time.smoothDeltaTime / slot._iMaxCoolTime;
            yield return null;
        }
        yield break;
    }

    public IEnumerator CoolDown(QuickSlot slot)
    {
        while (slot._iCurCoolTime > 0)
        {
            slot.CoolTimeRunTextUIUpdate();
            slot.TimerOneSec();
            yield return new WaitForSeconds(1.0f);
        }
        slot.CoolTimeRunUIUpdateEnd();
        yield break;
    }
    //�ش� ��ų ���̵� ������ �����Կ� ��ų ����Ҽ� �ִٰ� ����
    public void SetCanSkillUseBySkillID(int id)
    {
        foreach (SkillQuickSlot slot in m_quickSlotSkills)
        {
            if(id == slot._ID)
            {
                slot.SetCanUse();
            }
        }
    }
    
    //��ų �����Կ� ����
    public void SetSkillInSkillQuickSlot(int slotNum)
    {
        if (UIManager.Instance._QuickSlotUI.CanSelectCheckBySlotNum(slotNum))   // �ʷϻ� ���� ��������(�� �� �ִ� ���� ��
        {
            int sameIDSlotNum = IsSameIDReturnSlotNum(m_iSkillEquipmentID); 
            if (sameIDSlotNum == -1)    // ���� ���̵� ���� ��ų�� ���� �� -> ó�� ��ų���
            { 
                m_quickSlotSkills[slotNum].SetSkillDataInQuickSlot(m_iSkillEquipmentID);
                UIManager.Instance._QuickSlotUI.QuickSlotUIUpdate(slotNum, m_iSkillEquipmentID);

            }
            else // ���� ���̵� ���� ��ų�� ���� ��W
            {
                Debug.Log("���� ���̵� ���� ��ų�� ����" + slotNum + "  " + sameIDSlotNum); 
            }
                UIManager.Instance._QuickSlotUI.QuickSlotSelectOff();

        }

    }

    //��ų ���̵� ������ �ش� ��ų�� ������ �ִ� ���� ��ȣ ��ȯ
    public int IsSameIDReturnSlotNum(int id)
    {
        int idx = 0;
        foreach (SkillQuickSlot slot in m_quickSlotSkills)
        {
            if (id == slot._ID)
            {
                return idx;
            }
            idx++;
        }
        return -1;

    }

    //��ų �����Կ� 
    public void SwapSkillQuickSlot(int slotNum1, int slotNum2)
    {

            if (m_quickSlotSkills[slotNum1]._bIsEmpty)
            {

                m_quickSlotSkills[slotNum1].SetSkillDataInQuickSlot(m_quickSlotSkills[slotNum2]._ID);

                UIManager.Instance._QuickSlotUI.QuickSlotUIUpdate(slotNum1, m_quickSlotSkills[slotNum1]._ID);

                m_quickSlotSkills[slotNum2].SetEmpty();
                return;
            }

            SkillQuickSlot slot = m_quickSlotSkills[slotNum1];
            m_quickSlotSkills[slotNum1].SetSkillDataInQuickSlot(m_quickSlotSkills[slotNum2]._ID);
            m_quickSlotSkills[slotNum2].SetSkillDataInQuickSlot(slot._ID);

            UIManager.Instance._QuickSlotUI.QuickSlotUIUpdate(slotNum1, m_quickSlotSkills[slotNum1]._ID);
            UIManager.Instance._QuickSlotUI.QuickSlotUIUpdate(slotNum2, m_quickSlotSkills[slotNum2]._ID);

    }

    
}