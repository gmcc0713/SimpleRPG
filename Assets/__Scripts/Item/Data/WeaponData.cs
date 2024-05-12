using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Sword,
    Mace,
    Axe
}

public class WeaponData : EquipmentData
{
    public WeaponType weaponType;       //��������(��,Ȱ)
    public float AttackDamage;          //���� ������

    public void SetWeaponData(string[] datas)
    {
        weaponType = System.Enum.Parse<WeaponType>(datas[1]);
        AttackDamage = float.Parse(datas[2]);
        
    }
}