using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[System.Serializable]
public class SkillQuickSlot : QuickSlot
{
    public int _ID => m_iID;
    public override void Initialize(int i, string a)
    {
        m_iSlotNum = i;
        fillAmountValue = 0;
        m_bIsEmpty = true;
        code = a;
       

    }
    public void SetCanUse() { m_bCanUse = true; }
    public void SetSkillDataInQuickSlot(int skillID)           //아이템이 처음 퀵슬롯에 장착되었을때(퀵슬롯이 비어있을때)
    {
        m_iID = skillID;
        UIManager.Instance._QuickSlotUI.QuickSlotUIUpdate(m_iSlotNum,skillID);
        m_bIsEmpty = false;
        m_iMaxCoolTime = PlayerController.Instance._PlayerSkill.GetSkillByIndex(m_iID)._data.m_fCoolTime;
    }
    public void UpdateUI()
    {
        UIManager.Instance._QuickSlotUI.QuickSlotUIUpdate(m_iSlotNum, m_iID);
    }
    public override void IsPressedKey()
    {
        if (m_bIsEmpty || !m_bCanUse)
        {
            Debug.Log("cant skill use");
            return;
        }

        if (Input.GetKeyDown((KeyCode)Enum.Parse(typeof(KeyCode),code)))
        {
            m_bCanUse = false;

     
            PlayerController.Instance._PlayerSkill.SkillUse(m_iID);
            QuickSlotManager.Instance.CoolTimeStart(this);

        }

    }
    public void SetEmpty()
    {
        fillAmountValue = 0;
        m_bIsEmpty = true;
        m_iID = -1;
        UIManager.Instance._QuickSlotUI.RemoveSkill(m_iSlotNum);
    }

    public override void CoolTimeRunTextUIUpdate()
    {
        UIManager.Instance._QuickSlotUI.CoolDownTextUpdate(QuickSlotType.Skill, m_iSlotNum, ((int)m_iCurCoolTime).ToString());

    }
    public override void CoolTimeRunFillUIUpdate()
    {
        UIManager.Instance._QuickSlotUI.CoolDownFillImage(QuickSlotType.Skill,m_iSlotNum, fillAmountValue);
    }
    public override void CoolTimeRunUIUpdateEnd()
    {
        m_bCanUse = true;
        UIManager.Instance._QuickSlotUI.CoolDownFillImage(QuickSlotType.Skill,m_iSlotNum, 0);
        UIManager.Instance._QuickSlotUI.CoolDownTextUpdate(QuickSlotType.Skill, m_iSlotNum, "");

    }
}

