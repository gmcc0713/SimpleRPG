using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuickSlotUIController : MonoBehaviour
{
    [SerializeField] private ItemQuickSlotUI[] m_quickSlotItems;
    [SerializeField] private SkillQuickSlotUI[] m_quickSlotSkills;
    [SerializeField] private BuffTimerPanel m_buffTimerPanel;
    public void BuffSkillUseSetActiveTimer(Skill skill)
    {
        m_buffTimerPanel.BuffTimerOn(skill);
    }
    public void CoolDownFillImage(QuickSlotType type,int slotNum,float fillAmount)
    {
        switch (type)
        {
            case QuickSlotType.Skill:
                m_quickSlotSkills[slotNum].SetCoolDownFillImage(fillAmount);
                break;
            case QuickSlotType.Item:
                m_quickSlotItems[slotNum].SetCoolDownFillImage(fillAmount);
                break;
        }


    }
    public void CoolDownTextUpdate(QuickSlotType type,int slotNum, string curCooltime)
    {
        switch (type)
        {
            case QuickSlotType.Skill:
                m_quickSlotSkills[slotNum].SetCoolDownText(curCooltime);
                break;
            case QuickSlotType.Item:
                m_quickSlotItems[slotNum].SetCoolDownText(curCooltime);
                break;
        }

    }


    public void UpdateQuickSlot(QuickSlotType type,int slotNum,int id,int amount)
    {
        switch (type)
        {
            case QuickSlotType.Skill:
                m_quickSlotSkills[slotNum].SetUI(PlayerController.Instance._PlayerSkill.GetSkillByIndex(id)._data.m_SkillImage);
                break;
            case QuickSlotType.Item:
                m_quickSlotItems[slotNum].SetItemUI(id, amount);
                break;
        }

    }
    public void QuickSlotSelectOff()
    {
        foreach (SkillQuickSlotUI skill in m_quickSlotSkills)
        {
            skill.SelectBtnOff();
        }
    }
    public void SkillSlotSetColor(int slotNum, bool CanSelectOnColor) //�ش� �ε����� UI�� ���� �������� �Ѱ��༭ ����
    {
        m_quickSlotSkills[slotNum].SetColor(CanSelectOnColor);
    }
    public bool CanSelectCheckBySlotNum(int slotNum)
    {
       return !m_quickSlotSkills[slotNum].CanSelect();
    }
    public void QuickSlotUIUpdate(int idx, int id)
    {
        m_quickSlotSkills[idx].SetUI(PlayerController.Instance._PlayerSkill.GetSkillByIndex(id)._data.m_SkillImage);
 
 
    }

    //������â���� �������� �����Ǿ�����
    public void RemoveItem(int slotNum)
    {
        m_quickSlotItems[slotNum].SetEmpty();
    }
    public void RemoveSkill(int slotNum)
    {
        m_quickSlotSkills[slotNum].SetEmpty();
    }
}
