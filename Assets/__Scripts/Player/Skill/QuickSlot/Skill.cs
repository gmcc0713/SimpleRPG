using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Skill : MonoBehaviour
{
    private SkillData m_data;
    private SkillType m_skillType;
    public SkillType _skillType => m_skillType;
    private bool m_bCanUse;
    public SkillData _data => m_data;

    public virtual void Use() { }
    public void SetSkillType(SkillType skillType)
    {
        m_skillType = skillType;
    }
    public void SetDatas(string[] s)
    {
        m_skillType = SkillType.Locked;
        m_data = new SkillData();
        m_data.SetDatas(s);
        
    }
}
