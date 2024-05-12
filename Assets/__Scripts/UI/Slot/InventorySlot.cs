/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler,IEndDragHandler, IDropHandler
{
    [SerializeField] protected int slotNum;
    [SerializeField] protected SlotType type;
    protected bool isEmpty = true;
    protected int itemNum;


    protected Sprite baseImage;
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemAmountText;
    protected ItemToolTipUpdate itemToolTip;

    public bool _isEmpty=>isEmpty;
    public SlotType _type => type;
    public int _slotNum { get { return slotNum; } set { slotNum = value; } }
    //private bool m_bIsDragging;
    protected void Initialize()
    {
        baseImage = itemImage.sprite;

        if (type != SlotType.QuickSlot)
        {
            itemToolTip = GetComponent<ItemToolTipUpdate>();
            itemToolTip.SetData(itemNum, isEmpty);
        }
    }
    protected void Start()
    {

        Initialize();
    }

    public void OnBeginDrag(PointerEventData eventData)     // �巡�� ����
    {

        if (isEmpty)
        {
            return;
        }
        //m_bIsDragging = true;
        if (eventData.button.Equals(PointerEventData.InputButton.Left))
        {
            ViewIcon.instance.viewIcon = this;                              //�巡�� �ҋ��� ���� ���Կ� �ڱ��ڽ� ����
            ViewIcon.instance.DragSetImage(itemImage.sprite);
            ViewIcon.instance.transform.position = eventData.position;      //���콺�� ���� �ִ� ��ġ
        }

        if (ViewIcon.instance.viewIcon._type == SlotType.QuickSlot)
        {
            SetEmpty();
        }
    }

    public void OnDrag(PointerEventData eventData)  //�巡�� ����
    {
        if (isEmpty)
        {
            return;
        }
        if (eventData.button.Equals(PointerEventData.InputButton.Left))
        {
            ViewIcon.instance.transform.position = eventData.position;
        }
        this.GetComponentInParent<QuickSlotUI>();
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        ViewIcon.instance.SetColor(0);
        ViewIcon.instance.viewIcon = null;

    }

    public void OnDrop(PointerEventData eventData)      //�巡�� �� �ش� ��ġ�� �������� �� ȣ��(�������� �ڱ��ڽ� ���� ����������)
    {
        if(!ViewIcon.instance.viewIcon)  
        {
            ViewIcon.instance.SetColor(0);
            ViewIcon.instance.viewIcon = null;
            return;
        }

        switch(type)        //�ڱ��ڽ��� ���� ��������
        {
            case SlotType.Inventory:
                if (ViewIcon.instance.viewIcon._eSlotType == SlotType.Inventory)                      //�κ��丮 -> �κ��丮
                {
                    Debug.Log("Swap Iv to Iv -> new slotNum : " + slotNum + " base slotNum : " + ViewIcon.instance.viewIcon.slotNum);
                    Inventory.Instance.Swapitem(slotNum, ViewIcon.instance.viewIcon.slotNum);
                }
                else if (ViewIcon.instance.viewIcon._eSlotType == SlotType.Equipment)                //���â -> �κ��丮
                {
                    Equipment.Instance.UnEquipping(ViewIcon.instance.viewIcon.slotNum);
                }
                break;
            case SlotType.Equipment: //�κ��丮 -> ���â
                Equipment.Instance.Equipping(Inventory.Instance.FindItemBySlotNum(ViewIcon.instance.viewIcon.slotNum));
                break;
            case SlotType.QuickSlot: //�κ��丮 -> ������
                {
                    if (ViewIcon.instance.viewIcon._eSlotType == SlotType.Inventory)
                    {

                        Item item = Inventory.Instance.FindItemBySlotNum(ViewIcon.instance.viewSlotNum);
                        if (ItemDataManager.Instance.FindItem(item.id).itemtype == ItemType.Portion)
                        {
                            Debug.Log("�����Կ� ����");
                            SetItemUI(item.id, ((CountableItem)item).amount);
                            if(this is QuickSlot qSlot)
                            {
                                qSlot.SetQuickSlot(slotNum);
                            }
                            
                        }

                    }
                    break;
                }
            default:
                Debug.Log("�������� �ٸ����� ����߷�����");
                ViewIcon.instance.SetColor(0);
                ViewIcon.instance.viewIcon = null;
                break;
        }
        ViewIcon.instance.SetColor(0);
        ViewIcon.instance.viewIcon = null;
    }

    //������ ����

    //��������ġ��ȯ
    public void SetEmpty()
    {
        Debug.Log("Empty");
        itemImage.sprite = baseImage;
        isEmpty = true;

        itemAmountText.text = "";
        if(type != SlotType.QuickSlot)
            itemToolTip.SetEmpty(true);
    }
    public void SetItemUI(int id,int amount = -1)
    {
        itemNum = id;
        itemImage.sprite = ItemDataManager.Instance.FindItemImage(itemNum);
        isEmpty = false;
        if (type != SlotType.QuickSlot)
            itemToolTip.SetData(itemNum,isEmpty);

        if (ItemDataManager.Instance.CheckIsEquipmentItem(itemNum))
        {
            itemAmountText.text = "";
            return;
        }
        itemAmountText.text = amount.ToString();
    }
    public void UpdateSlot(int slotNum)
    {
        Item item = Inventory.Instance.FindItemBySlotNum(slotNum);
        itemNum = item.id;
        if (item is CountableItem cItem)
        {
            itemAmountText.text = cItem.amount.ToString();
        }
    }
}
*/