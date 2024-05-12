using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PortionItem : CountableItem
{
    
    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        id = -1;
        slotNum = -1;
        isEquipment = false;
        amount = 0;
    }
    public override void ItemUse()
    {
        Debug.Log("ItemUse");
        ItemData data = ItemDataManager.Instance.m_lItemDatas[id];
        PlayerController.Instance.UsePortionItem(((PortionData)data).m_eType, ((PortionData)data).m_iValue);
        QuickSlotManager.Instance.UseItem(data.itemId);
    }
}
//Use ����� ��� �����ʴ� ��������ü�� ����ִ� Use�� ���(system �޼��� ��½� �ش� Use �Լ��� ����)