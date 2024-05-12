using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemToolTip : MonoBehaviour
{
    //���� ������
    [SerializeField] Image iconImage;
    [SerializeField] TextMeshProUGUI itemNameText;

    [SerializeField] TextMeshProUGUI descriptionText;

    [SerializeField] TextMeshProUGUI sellPriceText;


    
    public void UpdateToolTip(ItemData data)
    {
        UpdateCommonData(data);

        switch (data.itemtype)
        {
            case ItemType.Weapon:
                UpdateWeaponText(data);
                break;
            case ItemType.Armor:
                UpdateArmorText(data);
                break;
            case ItemType.Portion:
                UpdatePortionText(data);
                break;
            default:
                UpdateEtcItemText(data);
                break;
        }
    }

    private void UpdateCommonData(ItemData data)
    {
        iconImage.sprite = data.itemIcon;
        itemNameText.text = data.itemName;
        itemNameText.text = AddString("<color=#996633>������ �̸�</color> \n", data.itemName);
        sellPriceText.text = AddString("�ǸŰ�� : ", (data.price*0.7f).ToString());
    }   

    void UpdateWeaponText(ItemData data)
    {
        WeaponData wData = (WeaponData)data;
        
        descriptionText.color = Color.red;
        itemNameText.text = AddString(itemNameText.text, "\n<color=#996633>����</color> \n", "����");
        descriptionText.text = AddString("<color=orange>�ʿ� ���� ", wData.equipmentLevel.ToString()," LV</color>");

        descriptionText.text = AddString(descriptionText.text,"\n���ݷ� : ", wData.AttackDamage.ToString());

    }
    void UpdateArmorText(ItemData data)
    {
       
        ArmorData aData = (ArmorData)data;

        itemNameText.text = AddString(itemNameText.text, "\n<color=#996633>����</color> \n", "��");
        descriptionText.text = AddString("<color=orange>�ʿ� ���� ", aData.equipmentLevel.ToString(), " LV</color>");

        descriptionText.color = Color.cyan;
        descriptionText.text = AddString(descriptionText.text,"\n�߰� ���� : ", aData.defenseValue.ToString());
        descriptionText.text = AddString(descriptionText.text,"\n�߰� ü�� : ", aData.healthValue.ToString());
    }
    void UpdatePortionText(ItemData data)
    {
        PortionData pData = (PortionData)data;

        itemNameText.text = AddString(itemNameText.text, "\n<color=#996633>����</color> \n", "����");
        descriptionText.color = Color.green;
        
        descriptionText.text = AddString("���� ȸ���� : ", pData.m_iValue.ToString());

    }
    void UpdateEtcItemText(ItemData data)
    {
        EtcItemData mData = (EtcItemData)data;

        itemNameText.text = AddString(itemNameText.text, "\n<color=#996633>����</color> \n", "��Ÿ");
        descriptionText.color = Color.white;

        descriptionText.text = AddString("���� : ", mData.itemDescription.ToString());

    }
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    public string AddString(string str1,string str2,string str3 ="")
    {
        System.Text.StringBuilder baseString = new System.Text.StringBuilder(str1);
        baseString.Append(str2);
        baseString.Append(str3);
        return baseString.ToString();
    }
}
