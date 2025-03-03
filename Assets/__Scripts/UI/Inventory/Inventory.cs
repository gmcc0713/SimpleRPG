using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class Inventory : MonoBehaviour
{
    public List<Item> inventoryItems;       //���� ���� ������ �ִ� ������
    [SerializeField] private int itemMaxAmount = 64;
    [SerializeField] private int SlotmaxCount = 24;
    [SerializeField] private int playerGold = 300000;
    public void initialize()
    {
        if(inventoryItems == null)
            inventoryItems = new List<Item>();
        UIManager.Instance._InventoryUI.UpdateGoldUI(playerGold);
        UpdateInventoryUIAll();
    }
    public bool BuyItem(int id, int gold)
    {
        if (playerGold < gold || SlotmaxCount <= inventoryItems.Count)
        {
            Debug.Log("��� ���� �̰ų� �κ��丮 ���� ��");
            return false;
        }

        AddGold(-gold);

        GetItem(id);
        return true;
    }
    public void AddGold(int addGold)
    {
        playerGold += addGold;
        UIManager.Instance._InventoryUI.UpdateGoldUI(playerGold);
        if (playerGold < 0)
        {
            playerGold = 0;
        }
    }

    public void GetItem(int itemID, int amount = 1, int upgrade = 0)    //�������� ������� ������������ �ƴ��� �Ǻ�
    {
        QuestManager.Instance.NotifyQuestUpdate(Quest_Type.Type_Collect, itemID, amount);

        if (ItemDataManager.Instance.CheckIsEquipmentItem(itemID))      //�ش� �������� ���������̿��� ��ġ�Ⱑ �Ұ����Ҷ�
        {
            AddEquipmentItem(itemID, upgrade);
            return;
        }
        else if(ItemDataManager.Instance.CheckCanUseItem(itemID))
        {
            AddPortionItem(itemID, amount);
            return;
        }
        AddCountableItem(itemID, amount);


    }
   
    public void ItemUse(int ivSlotNum,int quickSlotNum)
    {
        PortionItem pitem = inventoryItems[ivSlotNum] as PortionItem;
        if (pitem == null)
        {
            return;
        }
        pitem.ItemUse();
        pitem.amount--;

        UIManager.Instance._InventoryUI.UpdateSlotUI(pitem);

        QuickSlotManager.Instance.UpdateQuickSlotByQuickSlotNum(quickSlotNum);

        if (pitem.amount<=0)
        {
            UIManager.Instance._QuickSlotUI.RemoveItem(quickSlotNum);
            RemoveItem(ivSlotNum);
        }
    }
    public void AddPortionItem(int id, int amount)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (!inventoryItems[i].isEquipment)
            {
                if (inventoryItems[i] is PortionItem cItem)
                {
                    if (cItem.id == id && cItem.amount < itemMaxAmount)        //�ش�������� �̹� �����ϰ� �ִٸ� 
                    {
                        ChangeAmount(i, amount);            //������ ���� �߰�

                        ItemAmountOverCheck(i);                 //�ش�������� �ִ� ���� �Ѿ����� Ȯ��
                        return;
                    }
                }
            }
        }

        if (InventoryFullCheck())
        {
            return;
        }

        PortionItem item = new PortionItem();

        item.SetItem(id, amount, UIManager.Instance._InventoryUI.FindFrontEmptySlotIndex());
        inventoryItems.Add(item);
        UpdateSlot(item);
        QuickSlotManager.Instance.ItemAddQuickSlotUpdate(id);



    }
    public void AddCountableItem(int id, int amount)                    //���� �����ִ� �������� �������
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (!inventoryItems[i].isEquipment)
            {
                if (inventoryItems[i] is CountableItem cItem)
                {
                    if (cItem.id == id && cItem.amount < itemMaxAmount)        //�ش�������� �̹� �����ϰ� �ִٸ� 
                    {
                        ChangeAmount(i, amount);            //������ ���� �߰�
                        ItemAmountOverCheck(i);                 //�ش�������� �ִ� ���� �Ѿ����� Ȯ��

                        return;
                    }
                }
            }
        }

        if (InventoryFullCheck())
        {
            return;
        }

        CountableItem item = new CountableItem();
        item.SetItem(id, amount, UIManager.Instance._InventoryUI.FindFrontEmptySlotIndex());
        inventoryItems.Add(item);
        UpdateSlot(item);
       


    }
    public void AddEquipmentItem(int id, int upgrade)               //���������� �������
    {
        if (InventoryFullCheck())
        {
            Debug.Log("full");
            return;
        }
        EquipmentItem item = new EquipmentItem();

        item.SetItem(id, upgrade, UIManager.Instance._InventoryUI.FindFrontEmptySlotIndex(), true);       //����ִ� ���� ù���� ��ġã��
        inventoryItems.Add(item);
        
        UpdateSlot(item);
    }
    public bool InventoryFullCheck()                            //�κ��丮�� ���� á����
    {
        if (SlotmaxCount <= inventoryItems.Count)
        {
            return true;
        }
        return false;
    }
    public void FindAndRemoveItem(int itemid, int amount)              //������ ����
    {
        if (amount.Equals(-1))
        {
            Debug.Log("������ ���� ����");
            return;
        }
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].id == itemid)
            {
                int remain = ChangeAmount(i, -amount);
                if(remain<0)
                {
                    amount = -remain;
                }
                else
                {
                    RemoveItem(inventoryItems[i].slotNum);
                    break;
                }
            }
        }
    }
    
    public void RemoveItem(int slotNum)                             //�ش� ���Թ�ȣ�� ������ ������ ����
    {

        inventoryItems.Remove(inventoryItems[FindIndexBySlotNum(slotNum)]);     //������ ������ ����
        UIManager.Instance._InventoryUI.ClearSlot(slotNum);                                         //������ ���Կ� ����ִ� ������ ����
    }

    public int FindIndexBySlotNum(int slotNum)                      //���Թ�ȣ�� �κ��丮�� ������ �ִ� ������ ��ġ ã��
    {
        int index = -1;
        for(int i = 0;i< inventoryItems.Count;i++)
        {
            if (inventoryItems[i].slotNum == slotNum)
            {
                index = i;
                break;
            }
        }

        return index;
    }
    public int FindItemIdBySlotNum(int num)
    {
       ;
        return inventoryItems[FindIndexBySlotNum(num)].id;
    }
    public Item FindItemBySlotNum(int num)                          //���Թ�ȣ�� ������ ã��
    {
        Debug.Log("Item " + inventoryItems.Find(x => x.slotNum.Equals(num)));
        return inventoryItems.Find(x => x.slotNum.Equals(num));
    }
    public int FindItemIndexBySlotNum(int slotNum) //�ش� ���� ��ȣ�� �ִ� ��ġ�� ã��(�ش� ���� ��ȣ�� �����¾������̾����� -1 ��ȯ)
    {
        for (int i = 0; i < inventoryItems.Count; i++)                          //�ش� ������ ������ �ִ� �������� ������ ��ġ ã��
        {
            if (inventoryItems[i].slotNum == slotNum)
            {
                return i;
            }
        }
        return -1;
    }
    public int ChangeAmount(int index, int changeValue)
    {
        CountableItem updatedItem = (CountableItem)inventoryItems[index];
        //�ش� �������� 0���� �������� ������ ��ŭ ��ȯ�Ѵ�.
        if (updatedItem.amount + changeValue < 0)
        {
            return updatedItem.amount + changeValue;
        }
        updatedItem.amount += changeValue;

        inventoryItems[index] = updatedItem;
        UpdateSlot(inventoryItems[index]);
        return 0;
    }
    public void ChangeSlotNum(int index, int changeValue)               //���Թ�ȣ ����(������ ��ġ,�ٲ� ���Թ�ȣ)
    {
        inventoryItems[index].slotNum = changeValue;

        UpdateSlot(inventoryItems[index]);
    }

    public void ItemAmountOverCheck(int index)                  //�������� ��ġ�� �����ִ밹���� �Ѿ����� Ȯ��
    {
        CountableItem item = (CountableItem)inventoryItems[index];
        int overAmount = item.amount - itemMaxAmount;           //�ִ밹���� �Ѿ������� ���� ��(�� �������� ���� - �ִ밹�� = �����Ǵ� ����)

        if (overAmount > 0)                                     //�ִ밹���� �Ѿ�����
        {
            ChangeAmount(index, -overAmount);                   //�ִ밹���� ������ŭ ���� = �ִ밹���� �ȴ�

            item.SetItem(item.id, overAmount, UIManager.Instance._InventoryUI.FindFrontEmptySlotIndex());       //������ ���� ��
            inventoryItems.Add(item);                                                          //�������� ������ �߰�
            UpdateSlot(item);

            Debug.Log("Over");
            ItemAmountOverCheck(inventoryItems.Count-1);        //������ ����ŭ �ٽ� �����Ǿ����� üũ

            item.amount = overAmount;
        }
    }
    public void DieAndLostGold()
    {
        float gold = ((float)playerGold) * 0.9f;

        playerGold = (int)gold;
;

    }
    public void UpdateSlot(Item newItem)                        // ���� ������Ʈ
    {
        if (newItem.isEquipment)
        {
            UIManager.Instance._InventoryUI.UpdateSlotUI((EquipmentItem)newItem);
            return;
        }
        UIManager.Instance._InventoryUI.UpdateSlotUI((CountableItem)newItem);
    }

    
    public void Swapitem(int ChangeItem, int baseitem)
    {
        /*
          �����Ǿ����۰� ���� �� ��ġ �޾ƿ���
          ���� �������� ���â������ �̵� �϶��� �κ��丮 ������ ����ְų� ��� �������϶��� ��ȯ����
          ���� �ٸ� �������� �� ��ȯ ����
          ���� ��ĭ�Ͻ� ������ ��ȭ ����
        
          ������������ �κ��丮�������� �̵��϶��� ��ȯ
         */
        int index1, index2;                                    //������ �ִ� ������ ��ġ
        index1 = FindItemIndexBySlotNum(baseitem);            
        index2 = FindItemIndexBySlotNum(ChangeItem);          //�ش� ���� ��ȣ�� �ִ� ������ ã��

        if (UIManager.Instance._InventoryUI.EmptyCheck(ChangeItem))               //���� ����ִ°��� �巡�� �� ��ӵǾ�����
        {
            ChangeSlotNum(index1, ChangeItem);               //������ �ִ� ��ġ�� ���ιٲ� ���Թ�ȣ �߰�
            UpdateSlot(inventoryItems[index1]);             //���� ������ ���� ������Ʈ

            UIManager.Instance._InventoryUI.ClearSlot(baseitem);                //������ ���Թ�ȣ������ ����
            return;
        }
        
        if(inventoryItems[index1].id == inventoryItems[index2].id) 
        {
            if (inventoryItems[index1] is CountableItem cItem1 && inventoryItems[index2] is CountableItem cItem2)
            {
                if(itemMaxAmount > cItem2.amount + cItem1.amount)
                {
                    cItem2.amount += cItem1.amount;
                    RemoveItem(baseitem);
                    UpdateSlot(cItem2);

                }
                else
                {
                    cItem2.amount = itemMaxAmount;
                    cItem1.amount = cItem2.amount + cItem1.amount - itemMaxAmount;
                    UpdateSlot(cItem1);
                    UpdateSlot(cItem2);
                }
            }
            return;
        }
        ChangeSlotNum(index1, ChangeItem);
        ChangeSlotNum(index2, baseitem);

    }
    public void SwapSlot(int index1,int index2)
    {
        int slotTmp = inventoryItems[index1].slotNum;
        Item tmp = inventoryItems[index1];

        inventoryItems[index1] = inventoryItems[index2];
        inventoryItems[index1].slotNum = inventoryItems[index2].slotNum;

        inventoryItems[index2] = tmp;
        inventoryItems[index2].slotNum = slotTmp;


        UpdateSlot(inventoryItems[index1]);
        UpdateSlot(inventoryItems[index2]);
    }
    // �κ��丮 ���� �޼��� �߰� (����, ������ ��� ��)
    public void UpdateInventoryUIAll()
    {
        UIManager.Instance._InventoryUI.UpdateGoldUI(playerGold);
        foreach(var a in inventoryItems)
        {
            UIManager.Instance._InventoryUI.UpdateSlotUI(a);
        }
    }
    public void Divide(int slotNum,int amount)
    {
        if(inventoryItems[FindIndexBySlotNum(slotNum)] is CountableItem cItem)
        {
            cItem.amount -= amount;
            if (InventoryFullCheck())
            {
                return;
            }

            CountableItem item = new CountableItem();
            item.SetItem(cItem.id, amount, UIManager.Instance._InventoryUI.FindFrontEmptySlotIndex());
            inventoryItems.Add(item);
            UpdateSlot(item);
            UpdateSlot(cItem);  
        }
    }
}
