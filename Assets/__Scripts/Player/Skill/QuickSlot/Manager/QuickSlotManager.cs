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
    private int m_iSkillEquipmentID; //스킬 장착시 장착할 아이디
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



    public void ItemAddQuickSlotUpdate(int itemId) //입력받는 id 값의 아이템이 추가되어서 해당 아이템이 장착 되었는지 확인 및 업데이트
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
    public void SetItemInQuickSlot(int quickSlotNum, int ivSlotNum) //퀵슬롯의 번호를 받아 해당 번호의 퀵슬롯에 아이템 장착
    {
        m_quickSlotItems[quickSlotNum].UpdateItemSlotAddItem(ivSlotNum);
    }
    public void UpdateQuickSlotByQuickSlotNum(int quickSlotNum) //퀵슬롯의 번호를 입력받으면 해당 번호의 퀵슬롯만 업데이트 된다.
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
    public void CoolTimeStart(QuickSlot slot) //쿨타임 시작
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
    //해당 스킬 아이디를 가지는 퀵슬롯에 스킬 사용할수 있다고 전송
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
    
    //스킬 퀵슬롯에 저장
    public void SetSkillInSkillQuickSlot(int slotNum)
    {
        if (UIManager.Instance._QuickSlotUI.CanSelectCheckBySlotNum(slotNum))   // 초록색 불이 들어왔을때(고를 수 있는 곳일 때
        {
            int sameIDSlotNum = IsSameIDReturnSlotNum(m_iSkillEquipmentID); 
            if (sameIDSlotNum == -1)    // 같은 아이디를 가진 스킬이 없을 때 -> 처음 스킬등록
            { 
                m_quickSlotSkills[slotNum].SetSkillDataInQuickSlot(m_iSkillEquipmentID);
                UIManager.Instance._QuickSlotUI.QuickSlotUIUpdate(slotNum, m_iSkillEquipmentID);

            }
            else // 같은 아이디를 가진 스킬이 있을 때W
            {
                Debug.Log("같은 아이디를 가진 스킬이 있음" + slotNum + "  " + sameIDSlotNum); 
            }
                UIManager.Instance._QuickSlotUI.QuickSlotSelectOff();

        }

    }

    //스킬 아이디가 같을때 해당 스킬을 가지고 있는 슬롯 번호 반환
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

    //스킬 퀵슬롯에 
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