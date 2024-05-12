using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum ItemType
{
   Weapon = 0,           //���������
   Armor ,              //��������
   Portion ,            //���Ǿ�����
   Etc ,                //��Ÿ������
}
[System.Serializable]
public class ItemData 
{
    public ItemType itemtype;           //������ Ÿ��
    public int itemId;                  //������ ���̵�
    public string itemName;             //������ �̸�
    public Sprite itemIcon;             //������ ������
    public bool isEquipmentItem;        //�������� ���� ���� ����
    public int price;                   //������ �ǸŰ���
    public bool m_bCanUse;
    public int detailDataID;            //������ �� ���� ���̵�(���,���� �� �ٸ� ���̺��� ���̵�� �����´�)
    public virtual void SetData(string[] datas)
    {
        itemId = int.Parse(datas[0]);
        itemtype = System.Enum.Parse<ItemType>(datas[1]);
        itemName = datas[2];
        //�ε带 ������ �� ��� Ȯ�� �ؾߵȴ�(dictionary�� ���� �� Ű������ Ȯ��)
        //���̻� ��� �� �Ҷ� ���� �ʼ�(�̶� dictionary�� ���� ����)
        itemIcon = Resources.Load<Sprite>(datas[3]);   

        isEquipmentItem = bool.Parse(datas[4]);
        m_bCanUse = bool.Parse(datas[5]);
        price = int.Parse(datas[6]);
        detailDataID = int.Parse(datas[7]);
    }
}
