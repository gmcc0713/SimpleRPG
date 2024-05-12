using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillData
{
    public int m_iSkillID;
    public Sprite m_SkillImage;
    public string m_sSkillName;
    public float m_fCoolTime;
    public int m_beforeSkillID;
    public string m_sSkillDescription;
    public float m_fMP;

    public int m_iActiveTime;

    public void SetDatas(string[] s)
    {
        m_iSkillID = int.Parse(s[0]);
        m_SkillImage = Resources.Load<Sprite>(s[1]); ;
        m_sSkillName = s[2];
        m_fCoolTime = int.Parse(s[3]);
        m_beforeSkillID = int.Parse(s[4]);
        m_sSkillDescription = s[5];
        m_fMP = float.Parse(s[6]);
        m_iActiveTime = int.Parse(s[7]);

    }
}
