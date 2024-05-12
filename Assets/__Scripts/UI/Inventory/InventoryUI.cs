using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private List<ItemSlotUI> m_itemSlots;
    [SerializeField] private GameObject m_moveBar;
    [SerializeField] private TextMeshProUGUI m_inventoryGold;
    [SerializeField] private DivideItem m_DivideItem;
    public void Initialize()
    {
        for (int i = 0; i < m_itemSlots.Count; i++)
        {
            m_itemSlots[i].SetItemSlot(i); 
        }
    }
    public int FindFrontEmptySlotIndex()    //���° ������ ����ִ� ��������
    {
        for(int i = 0; i< m_itemSlots.Count;i++)
        {
            if (m_itemSlots[i]._bIsEmpty)       //������ ���������
            {
                Debug.Log("����ִ� ���� ã�� : " +  i);
                return i;
            }
        }
        return -1;

    }
    public void UpdateSlotUI(Item newitem)    //���� ������Ʈ
    {
        if (newitem.isEquipment)
        {
            m_itemSlots[newitem.slotNum].SetItemUI(newitem.id);
            return;
        }
        if (newitem is CountableItem cItem)
        {
            m_itemSlots[cItem.slotNum].SetItemUI(cItem.id, cItem.amount);
        }
        
    }
 
    public void ClearSlot(int slotIndex)    //���� ����
    {
        if (m_itemSlots[slotIndex])
            m_itemSlots[slotIndex].SetEmpty();
    }
    public bool EmptyCheck(int index)
    {
        return m_itemSlots[index]._bIsEmpty;
    }
    public void UpdateGoldUI(int gold)
    {
        m_inventoryGold.text = gold.ToString();
    }
    public void SelectModeOnOff(bool b)
    {
        foreach(ItemSlotUI item in m_itemSlots)
        {

                item.SelectBtnOn(b);

        }
    }
    public void DivideItem(int slotNum)
    {
        Item item = PlayerController.Instance._Inventory.FindItemBySlotNum(slotNum);
        if (item is CountableItem cItem)
        {
            m_DivideItem.DIvideAmountSelect(slotNum, cItem.amount);
        }
    }
}
