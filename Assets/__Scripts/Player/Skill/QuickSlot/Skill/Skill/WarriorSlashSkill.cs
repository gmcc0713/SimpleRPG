using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSlashSkill : Skill
{
    public Transform m_target;

    public override void Use()
    {
        PlayerController.Instance._PlayerSkill.WarriorSkillUse(_data.m_iActiveTime, _data.m_fMP,2);
        AudioManager.Instance.PlaySFX(4,0.1f);
    }
}
