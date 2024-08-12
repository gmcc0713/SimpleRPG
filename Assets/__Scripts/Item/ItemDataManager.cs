using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class ItemDataManager : MonoBehaviour
{
    public static ItemDataManager Instance { get; private set; }

    public List<ItemData> m_lItemDatas;

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
    private void Start()
    {
    }
    public void Initialize()
    {
        if(m_lItemDatas == null)
        {
            m_lItemDatas = new List<ItemData>();
        }
        LoadItemData();
        
    }
    public void LoadItemData()
    {
        List< string[]> itemDatas = CSVLoadManager.Instance.Load("CSV/Item/ItemDatas");
        List< string[]> equipmentDatas = CSVLoadManager.Instance.Load("CSV/Item/Equipment/EquipmentDatas");
        List< string[]> weaponDatas = CSVLoadManager.Instance.Load("CSV/Item/Equipment/WeaponDatas");
        List< string[]> armorDatas = CSVLoadManager.Instance.Load("CSV/Item/Equipment/ArmorDatas");

        List< string[]> potionDatas = CSVLoadManager.Instance.Load("CSV/Item/PotionDatas");
        List< string[]> etcDatas = CSVLoadManager.Instance.Load("CSV/Item/ETCDatas");
        for( int i =0; i < itemDatas.Count; i++)
        {
            switch (itemDatas[i][1])
            {
                case "Weapon":
                    {

                        WeaponData data = new WeaponData();
                        data.SetData(itemDatas[i]);
                        data.SetEquipmentData(equipmentDatas[data.detailDataID]);
                        data.SetWeaponData(weaponDatas[data.detailID]);
                        m_lItemDatas.Add(data);
                        break;
                    }
                case "Armor":
                    {
                        ArmorData data = new ArmorData();
                        data.SetData(itemDatas[i]);
                        data.SetEquipmentData(equipmentDatas[data.detailDataID]);
                        data.SetArmorData(armorDatas[data.detailID]);
                        m_lItemDatas.Add(data);
                        break;
                    }
                case "Portion":
                    {
                        PortionData data = new PortionData();
                        data.SetData(itemDatas[i]);
                        data.SetPortionData(potionDatas[data.detailDataID]);
                        m_lItemDatas.Add(data);
                        break;
                    }
                case "Etc":
                    {
                        EtcItemData data = new EtcItemData();
                        data.SetData(itemDatas[i]);
                        data.SetETCData(etcDatas[data.detailDataID]);
                        m_lItemDatas.Add(data);
                        break;
                    }
                default:
                    Debug.Log("¾ÆÀÌÅÛ È¹µæ ¿À·ù");
                    break;
            }
        }
    }

    public ItemData FindItem(int num)
    {
        return m_lItemDatas[num];
    }
    public Sprite FindItemImage(int num)
    {
        return m_lItemDatas[num].itemIcon;
    }
    public bool CheckIsEquipmentItem(int num)
    {

        return m_lItemDatas[num].isEquipmentItem;
    }
    public bool CheckCanUseItem(int num)
    {
        return m_lItemDatas[num].m_bCanUse;
    }
    public EquipmentType CheckEquipmenttype(int num)
    {

        if (!m_lItemDatas[num].isEquipmentItem)
        {
            return EquipmentType.Non;
        }
        Debug.Log((EquipmentData)m_lItemDatas[num]);
        EquipmentData equipmentData = (EquipmentData)m_lItemDatas[num];
        return equipmentData.equipmentType;
    }
    public int FindApperanceID(int num)
    {

        if (!m_lItemDatas[num].isEquipmentItem)
        {
            return -1;
        }
        EquipmentData equipmentData = (EquipmentData)m_lItemDatas[num];
        return equipmentData.appearanceID;
    }

}
