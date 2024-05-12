using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorFlyAttackSkill : Skill
{

    public override void Use()
    {
        PlayerController.Instance._PlayerSkill.WarriorSkillUse(_data.m_iActiveTime, _data.m_fMP,3);
        AudioManager.Instance.PlaySFX(5,1.0f);
    }
}
