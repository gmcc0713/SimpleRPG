using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private List<int> items = new List<int>();             //�������� �Ǹ��� ������ ��
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform parent;
    [SerializeField] private ItemToolTip toolTip;               

    private List<ShopSlot> slots = new List<ShopSlot>();

    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        MakeShopSlot(items.Count);
        Slotinitialize();
        SlotUpdate();
    }
    public void MakeShopSlot(int count)
    {
        for(int i = 0; i < count; i++)
        {
            //������Ʈ Ǯ������
            GameObject clone = Instantiate(slotPrefab);
            clone.transform.SetParent(parent.transform, false);

            ShopSlot slot = clone.GetComponent<ShopSlot>();
            slots.Add(slot);
        }
    }
    public void Slotinitialize()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetToolTipUpdater(toolTip);

            slots[i].SetData(items[i]);     //���� �ʱ⼼��
        }
    }
    public void SlotUpdate()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].UpdateSlot();
        }
    }
    //items�� ����ִ� ���� ���� ����� �ڵ�

    //������� ���� ���Կ� �����۵��� �ִ� �ڵ�




}
