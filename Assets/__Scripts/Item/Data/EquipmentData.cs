using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EquipmentData : ItemData
{
    public EquipmentType equipmentType;         //��� Ÿ��(ex) ����, ����, ���� ��)
    public int equipmentLevel;                  //��� ���� ���� ( ex) 20Lv�̻�, 30LV �̻� ��)
    public int appearanceID;
    public int detailID;                        //����� ������ ���̵�
    
    public EquipmentData()
    {


    }
    public void SetEquipmentData(string[] datas)
    {
        equipmentType = System.Enum.Parse<EquipmentType>(datas[1]);
        equipmentLevel = int.Parse(datas[2]);
        appearanceID = int.Parse(datas[3]);
        detailID = int.Parse(datas[4]);
    }

}
