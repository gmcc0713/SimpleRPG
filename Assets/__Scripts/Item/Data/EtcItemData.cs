using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EtcItemData : ItemData
{
    public string itemDescription;      //������ ����
    public void SetETCData(string[] datas)
    {
        itemDescription = datas[1];
    }
}

