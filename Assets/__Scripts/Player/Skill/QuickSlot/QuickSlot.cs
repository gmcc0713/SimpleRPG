using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuickSlot
{
    protected int m_iID = -1; //��ų ���̵�
    protected int m_iSlotNum = -1; //�������� ��ȣ
    [SerializeField] protected string code;
    protected float m_iCurCoolTime;
    protected float m_iMaxCoolTime;       //��Ÿ�� �ִ�ġ
    [SerializeField] protected bool m_bIsEmpty;
    [SerializeField] protected bool m_bCanUse = true;
    public float fillAmountValue;


    public bool _bIsEmpty => m_bIsEmpty;
    public int _iSlotNum => m_iSlotNum;
    public float _iCurCoolTime => m_iCurCoolTime;
    public float _iMaxCoolTime => m_iMaxCoolTime;
    public virtual void Initialize(int i, string a) { }
    public virtual void IsPressedKey() { }
    public virtual void CoolTimeRunTextUIUpdate() { }
    public virtual void CoolTimeRunFillUIUpdate() { }
    public virtual void CoolTimeRunUIUpdateEnd(){}
    public void TimerOneSec()
    {
        m_iCurCoolTime--;
    }
    public void StartTimer()
    {
        fillAmountValue = 1;
        m_iCurCoolTime = m_iMaxCoolTime;
    }
}
