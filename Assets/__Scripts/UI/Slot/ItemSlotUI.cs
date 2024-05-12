using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] protected Sprite m_baseImage;
    [SerializeField] protected Image m_itemImage;
    [SerializeField] protected TextMeshProUGUI m_itemAmountText;
    [SerializeField] protected ItemToolTipUpdate itemToolTip;
    public bool m_bIsEmpty = true;
    protected int m_iSlotNum;
    [SerializeField] protected SlotType m_eSlotType;
    [SerializeField] protected Button m_selectButton;

    private bool M_bCanDivide;
    public SlotType _eSlotType => m_eSlotType;
    public int _iSlotNum => m_iSlotNum;
    public bool _bIsEmpty => m_bIsEmpty;

    public void SetItemSlot(int num)
    {
        m_iSlotNum = num;
        m_bIsEmpty = true;
    }
     
    void Start()
    {
        Initialize();

    }
    void Initialize()
    {
    }
    public void OnBeginDrag(PointerEventData eventData)     // �巡�� ����
    {
        Debug.Log("������ �巡�� ����");
        if (m_bIsEmpty)
        {
            return;
        }
        //m_bIsDragging = true;
        if (eventData.button.Equals(PointerEventData.InputButton.Left))
        {
            ViewIcon.instance.viewIcon = this;                              //�巡�� �ҋ��� ���� ���Կ� �ڱ��ڽ� ����
            ViewIcon.instance.DragSetImage(m_itemImage.sprite);               //�巡�� �Ǵ� �̹��� �߰�
            ViewIcon.instance.transform.position = eventData.position;      //���콺�� ���� �ִ� ��ġ
        }

        if (ViewIcon.instance.viewIcon._eSlotType == SlotType.QuickSlot)
        {
            SetEmpty();
        }
    }
    public void OnDrag(PointerEventData eventData)  //�巡�� ����
    {
        if (m_bIsEmpty)
        {
            return;
        }
        if (eventData.button.Equals(PointerEventData.InputButton.Left))
        {
            ViewIcon.instance.transform.position = eventData.position;
        }
        this.GetComponentInParent<ItemQuickSlotUI>();
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        ViewIcon.instance.SetColor(0);
        ViewIcon.instance.viewIcon = null;

    }

    public void OnDrop(PointerEventData eventData)      //�巡�� �� �ش� ��ġ�� �������� �� ȣ��(�������� �ڱ��ڽ� ���� ����������)
    {
        if (!ViewIcon.instance.viewIcon)
        {
            ViewIcon.instance.SetColor(0);
            ViewIcon.instance.viewIcon = null;
            return;
        }

        switch (m_eSlotType)        //�ڱ��ڽ��� ���� ��������
        {
            case SlotType.Inventory:
                if (ViewIcon.instance.viewIcon._eSlotType == SlotType.Inventory)                      //�κ��丮 -> �κ��丮
                {
                    Debug.Log("Swap Iv to Iv -> new slotNum : " + m_iSlotNum + " base slotNum : " + ViewIcon.instance.viewIcon._iSlotNum);
                    PlayerController.Instance._Inventory.Swapitem(m_iSlotNum, ViewIcon.instance.viewIcon.m_iSlotNum);
                }
                else if (ViewIcon.instance.viewIcon._eSlotType == SlotType.Equipment)                //���â -> �κ��丮
                {
                    Equipment.Instance.UnEquipping(ViewIcon.instance.viewIcon._iSlotNum);
                }
                break;
            case SlotType.Equipment: //�κ��丮 -> ���â
                Equipment.Instance.Equipping(PlayerController.Instance._Inventory.FindItemBySlotNum(ViewIcon.instance.viewIcon._iSlotNum));
                break;
            case SlotType.QuickSlot: //�κ��丮 -> ������
                {
                    //�������� ������ �� ���� �� �����ϱ�
                   if (ViewIcon.instance.viewIcon._eSlotType == SlotType.Inventory) //�巡�� ���۽��� ���� Ÿ���� Inventory�϶�
                    {
                        Item item = PlayerController.Instance._Inventory.FindItemBySlotNum(ViewIcon.instance.viewSlotNum);
                        if (ItemDataManager.Instance.FindItem(item.id).itemtype == ItemType.Portion)    //�ش� �������� ���������ϸ�
                        {
                            Debug.Log("�����Կ� ����");
                            if(item is PortionItem pItem)
                            SetItemUI(pItem.id, pItem.amount);
                            QuickSlotManager.Instance.SetItemInQuickSlot(m_iSlotNum, ViewIcon.instance.viewIcon._iSlotNum);

                        }

                    }
                    break;
                }
            default:

                ViewIcon.instance.SetColor(0);
                ViewIcon.instance.viewIcon = null;
                break;
        }
        ViewIcon.instance.SetColor(0);
        ViewIcon.instance.viewIcon = null;
    }
    public void SetEmpty()
    {
        
        m_itemImage.sprite = m_baseImage;
        m_bIsEmpty = true;

        m_itemAmountText.text = "";
        if (m_eSlotType != SlotType.QuickSlot)
            itemToolTip.SetEmpty(true);
    }
    public void SetItemUI(int id, int amount = -1)
    {

        m_bIsEmpty = false;
        m_itemImage.sprite = ItemDataManager.Instance.FindItemImage(id);
        if (m_eSlotType != SlotType.QuickSlot)
            itemToolTip.SetData(id, m_bIsEmpty);

        if (ItemDataManager.Instance.CheckIsEquipmentItem(id))
        {
            m_itemAmountText.text = "";
            return;
        }
        m_itemAmountText.text = amount.ToString();
    }

    public void SelectBtnOn(bool b)
    {
        if(b)
        {
            m_selectButton.gameObject.SetActive(true);
            Item item = PlayerController.Instance._Inventory.FindItemBySlotNum(m_iSlotNum);
            if (item is CountableItem cItem)
            {
                if (cItem.amount > 1)
                {
                    m_selectButton.image.color = new Color(0, 1, 0, 0.2f);
                    M_bCanDivide = true;
                    return;
                }
            }
            m_selectButton.image.color = new Color(1, 0, 0, 0.2f);
            M_bCanDivide = false;
            return;
        }
        m_selectButton.gameObject.SetActive(false);

    }
    public void DivideBtnClickCheck()
    {
        if(!M_bCanDivide)
        {
            return;
        }
        Item item = PlayerController.Instance._Inventory.FindItemBySlotNum(m_iSlotNum);
        if (item is CountableItem cItem && !m_bIsEmpty)
        {
            UIManager.Instance._InventoryUI.DivideItem(m_iSlotNum);
        }
    }
    /*public void UpdateSlot(int inventoryNumber)
    {
        Item item = Inventory.Instance.FindItemBySlotNum(slotNum);
        SetItemUI()
        itemNum = item.id;

    }*/
}
