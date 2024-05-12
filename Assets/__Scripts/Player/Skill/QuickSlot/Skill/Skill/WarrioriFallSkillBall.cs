using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorFallSkillBall : Skill
{
    public Transform m_target;

    public override void Use()
    {
        PlayerController.Instance._PlayerSkill.WarriorSkillUse(_data.m_iActiveTime, _data.m_fMP,4);
        AudioManager.Instance.PlaySFX(6,1.0f);
    }
}
