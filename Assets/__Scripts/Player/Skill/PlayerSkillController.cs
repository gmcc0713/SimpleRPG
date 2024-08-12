using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
[System.Serializable]
public enum SkillType
{
    Locked,
    UnLocked,
    Active,
}
public class PlayerSkillController : MonoBehaviour
{
    List<Skill> m_skills;           //슬롯에 대응하는 스킬
    private int m_iSkillPoint;

    public void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        m_skills = new List<Skill>();
        SKillInit();
        

        m_iSkillPoint = 5;
        UIManager.Instance._skillUIController.UpdateSkillPointUI(m_iSkillPoint);
        bool first = true;
        foreach (Skill skill in m_skills) 
        {
            if(first)
            {
                skill.SetSkillType(SkillType.UnLocked);
                first = false;
            }
            UIManager.Instance._skillUIController.UpdateUI(skill);
        }
    }
    public void SKillInit()
    {
        List<string[]> skillDatas = CSVLoadManager.Instance.Load("CSV/Skill/SkillDatas");

        Skill clone = new WarriorBuffSkill();
        clone.SetDatas(skillDatas[0]);
        m_skills.Add(clone);

        clone = new WarriorSlashSkill();
        clone.SetDatas(skillDatas[1]);
        m_skills.Add(clone);

        clone = new WarriorFlyAttackSkill();
        clone.SetDatas(skillDatas[2]);
        m_skills.Add(clone);

        clone = new WarriorFallSkillBall();
        clone.SetDatas(skillDatas[3]);
        m_skills.Add(clone);

        clone = new WarriorBarrierSkill();
        clone.SetDatas(skillDatas[4]);
        m_skills.Add(clone);
    }
    //스킬을 배운뒤 해당 스킬과 연결된 스킬해금
    public void UnLockLinkedSkillByLearnSkill()
    {
        foreach (Skill skill in m_skills)
        {
            if (skill._data.m_beforeSkillID == -1)
                continue;
            if (skill._skillType == SkillType.Locked && m_skills[skill._data.m_beforeSkillID]._skillType == SkillType.Active)
            {
                skill.SetSkillType(SkillType.UnLocked);
                UIManager.Instance._skillUIController.UpdateUI(skill);
            }
        }
    }
    public void UpdateSkillAll()
    {
        UIManager.Instance._skillUIController.UpdateSkillPointUI(m_iSkillPoint);
        foreach (Skill skill in m_skills)
        {
            UIManager.Instance._skillUIController.UpdateUI(skill);
        }
    }
    public void SkillUse(int id)
    {
        m_skills[id].Use();
    }
    public  void SkillLearn(int id)
    {
        if (m_iSkillPoint <= 0)
            return;
        if (m_skills[id]._skillType == SkillType.UnLocked)
        {
            m_iSkillPoint--;
            m_skills[id].SetSkillType(SkillType.Active);
            UIManager.Instance._skillUIController.UpdateUI(m_skills[id]);
            UIManager.Instance._skillUIController.UpdateSkillPointUI(m_iSkillPoint);
        }

        UnLockLinkedSkillByLearnSkill();
    }
    //인덱스 번호로 스킬 가져오기
    public Skill GetSkillByIndex(int idx)
    {
        return m_skills[idx];
    }

    public void WarriorSkillUse(float coolTime, float mp,int skillNum)
    {
        PlayerController.Instance.AddMP(-mp);
        PlayerController.Instance._playerAnim.SetInteger("SkillNum", skillNum);
        PlayerController.Instance._playerAnim.SetTrigger("CanSkill");

        StartCoroutine(CoolDownStart(coolTime));
    }
    public IEnumerator CoolDownStart(float coolTime)
    {
        PlayerController.Instance.SetCanAct(false);
        yield return new WaitForSeconds(coolTime);

    }
    public void WarriorBarrierSkillUse(float coolTime, float mp)
    {
        PlayerController.Instance.AddMP(-mp);
        PlayerController.Instance.SetCantDamaged(true);
    }
    public void WarriorBuffSkillUse(float coolTime,float mp)
    {

        PlayerController.Instance.AddMP(-mp);
        PlayerController.Instance._playerAnim.SetInteger("SkillNum", 1);
        PlayerController.Instance._playerAnim.SetTrigger("CanSkill");
        StartCoroutine(WarriorBuffSkill(coolTime));
    }
    public IEnumerator WarriorBuffSkill(float coolTime)
    {
        PlayerController.Instance.SetAdditionalDamage(100);
        yield return new WaitForSeconds(coolTime);
        PlayerController.Instance.SetAdditionalDamage(-100);

    }
}
