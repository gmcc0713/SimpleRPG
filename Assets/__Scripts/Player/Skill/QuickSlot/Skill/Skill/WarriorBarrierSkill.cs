using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorBarrierSkill : Skill
{
    public Transform m_target;

    public override void Use()
    {
        PlayerController.Instance._PlayerSkill.WarriorBarrierSkillUse(_data.m_iActiveTime, _data.m_fMP);
        UIManager.Instance._QuickSlotUI.BuffSkillUseSetActiveTimer(this);
        AudioManager.Instance.PlaySFX(3);
    }
}
