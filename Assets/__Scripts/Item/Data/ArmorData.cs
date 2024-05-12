using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArmorData : EquipmentData
{
    public float defenseValue;              //�߰� ����
    public float healthValue;               //�߰� ü��
    public void SetArmorData(string[] datas)
    {
        defenseValue = float.Parse(datas[0]);
        healthValue = float.Parse(datas[1]);
    }

}
