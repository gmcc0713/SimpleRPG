using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemQuickSlot : QuickSlot
{
    private List<int>  m_iLinkedIVSlots;  //연결된 해당 인벤토리의 슬롯번호
    public override void Initialize(int i,string a)
    {
        m_iSlotNum = i;
        fillAmountValue = 0;
        m_bIsEmpty = true;
        m_iLinkedIVSlots = new List<int>();
        code = a;
        
    }
 
    public bool IsItemIDSame(int id) //입력받는 아이디랑 나의 퀵슬롯의 아이디가 같은지
    {
        return !m_bIsEmpty && id == PlayerController.Instance._Inventory.FindItemBySlotNum(m_iLinkedIVSlots[0]).id;
    }
    void SetItemDataInQuickSlot()           //아이템이 처음 퀵슬롯에 장착되었을때(퀵슬롯이 비어있을때)
    {
        m_bIsEmpty = true;
        Item item = PlayerController.Instance._Inventory.FindItemBySlotNum(m_iLinkedIVSlots[0]);
        if(ItemDataManager.Instance.m_lItemDatas[item.id] is PortionData pItemData)
        {
            if (item is PortionItem pItem)
            {
                m_iMaxCoolTime = pItemData.m_iCoolTime;
            }
        }
      

    }

    public void UpdateItemSlotAddItem(int slotNum)   //새로운 아이템 추가
    {
        if(!m_bIsEmpty) //비어있지 않다면
        {
            RemoveItemData();   //퀵슬롯 초기화
        }
        m_iLinkedIVSlots.Add(slotNum);

        SetItemDataInQuickSlot();


        UpdateItemInLinkedIvslot();
        m_bIsEmpty = false;
        
        return;
    }
    public void RemoveItemData()
    {
        m_bIsEmpty = true;
        m_iLinkedIVSlots.Clear();
        m_iMaxCoolTime = 0;
        m_bCanUse = true;

    }

    //연결된 인벤토리 슬롯에 있는 모든 아이템 갯수 합치고  UI Update하기
    public void UpdateItemInLinkedIvslot()
    {
        m_bIsEmpty = false;
        int amount = 0;
        int id = -1;
        foreach(int num in m_iLinkedIVSlots)
        {
            Item item = PlayerController.Instance._Inventory.FindItemBySlotNum(num);
            id = item.id;
            if (item is PortionItem pItem)
            {
                amount += pItem.amount;
            }
        }
        UIManager.Instance._QuickSlotUI.UpdateQuickSlot(QuickSlotType.Item, m_iSlotNum, id, amount);
    }

    public override void IsPressedKey()
    {
        if (m_bIsEmpty || !m_bCanUse)
        {
            return;
        }
        if (Input.GetKeyDown(code))
        {
            m_bCanUse = false;
            PlayerController.Instance._Inventory.ItemUse(m_iLinkedIVSlots[0],m_iSlotNum);
            QuickSlotManager.Instance.CoolTimeStart(this);
        }

    }


    public override void CoolTimeRunTextUIUpdate()
    {
        UIManager.Instance._QuickSlotUI.CoolDownTextUpdate(QuickSlotType.Item, m_iSlotNum, ((int)m_iCurCoolTime).ToString());

    }
    public override void CoolTimeRunFillUIUpdate()
    {
        UIManager.Instance._QuickSlotUI.CoolDownFillImage(QuickSlotType.Item, m_iSlotNum, fillAmountValue);
    }
    public override void CoolTimeRunUIUpdateEnd()
    {
        m_bCanUse = true;
        UIManager.Instance._QuickSlotUI.CoolDownFillImage(QuickSlotType.Item, m_iSlotNum, 0);
        UIManager.Instance._QuickSlotUI.CoolDownTextUpdate(QuickSlotType.Item, m_iSlotNum, "");

    }
}
