using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_warriorBuff;
    [SerializeField] private Animator m_animator;

    [SerializeField] private int m_skillPoint;
    public int _SkillPoint => m_skillPoint;
    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        m_skillPoint = 1;
    }
    public void SkillPointUse()
    {
        Debug.Log(m_skillPoint);
        m_skillPoint --;
    }
    public void SkillUse(int id)
    {
        m_animator.SetInteger("SkillNum", id);
        switch (id)
        {
            case 1:
                WarriorBuffSkillID1();
                break;
        }

    }
    public void WarriorBuffSkillID1()
    {
    }
}
