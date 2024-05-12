using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Non = -1,
    Weapon = 0,
    Helmet,
    Armor,
    Pants,
    Shoes,
    Hand,
    Ring,
    Nacklace,

    Count   //enum ���� Ȯ��
}

public class Equipment : MonoBehaviour
{
    public static Equipment Instance { get; private set; }
    void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }

    public EquipmentItem[] equipmentItems = new EquipmentItem[(int)EquipmentType.Count];       //�� �����Ҽ� �ִ� ������ ����
    [SerializeField] private CharacterAppearanceManager characterAppearanceManager;
    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        UpdateAllItemUI();
    }

    public void Equipping(Item item)
    {
        if(!item.isEquipment)   
        {
            return;
        }
        EquipmentItem equipmentItem = (EquipmentItem)item;
        EquipmentType type = ItemDataManager.Instance.CheckEquipmenttype(equipmentItem.id);  //�ش� �������� Ÿ�� Ȯ��(����, ���� ��)

        if (!UIManager.Instance._EquipmentUI.EmptyCheckSlot((int)type))    //���� �ش� ������ ��������ʴٸ�
        {
            PlayerController.Instance._Inventory.GetItem(equipmentItems[(int)type].id, 1, equipmentItems[(int)type].upgrade);
            
           Debug.Log("�ش� ���� ������� �ʾ� �κ��丮�� �ٲ����");
        }

        PlayerController.Instance._Inventory.RemoveItem(item.slotNum);  //�κ��丮 ������ ������ ����
        Debug.Log(type.ToString());
        equipmentItems[(int)type] = equipmentItem;      //�ش� �������� �߰�
        equipmentItems[(int)type].slotNum = (int)type;  //�ش� ������ ���Թ�ȣ �߰�
        characterAppearanceManager.ChangeParts(type, ItemDataManager.Instance.FindApperanceID(item.id));//������ȭ
        UIManager.Instance._EquipmentUI.UpdateItem(item.id, (int)type);
        PlayerController.Instance.SetWeaponDamage(((WeaponData)ItemDataManager.Instance.FindItem(item.id)).AttackDamage);
        

    }
    void ChangeEquipmentItemAndApplyAdditionalStats()
    {
    }
    public void UnEquipping(int slotNum)
    {
        PlayerController.Instance._Inventory.GetItem(equipmentItems[(int)slotNum].id, 1, equipmentItems[(int)slotNum].upgrade);

        equipmentItems[slotNum] = null;
        characterAppearanceManager.ChangeParts((EquipmentType)slotNum, 0);
        UIManager.Instance._EquipmentUI.ClearEquipmentSlot(slotNum);

    }
    public void UpdateAllItemUI()
    {
        foreach(var data in equipmentItems)
        {
            if(data != null)
                UIManager.Instance._EquipmentUI.UpdateItem(data.id, data.slotNum);
        }
    }
}