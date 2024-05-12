using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }
    void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }
    //==================================================================================
    List<Skill> m_SkillList;
    public void Start()
    {
        Initialize();
    }
    public void Initialize()
    {


        m_SkillList = new List<Skill>();
        List<string[]> skillDatas = SCVLoadManager.Instance.Load("CSV/Skill/SkillDatas");
        m_SkillList.Add(new WarriorBuffSkill());
        m_SkillList[0].SetDatas(skillDatas[0]);

        /*  foreach (string[] skillData in skillDatas)
          {
              Skill clone = new Skill();
              clone.SetDatas(skillData);
              m_SkillList.Add(clone);
          }*/

    }
    public void SkillUse(int id)
    {
        Debug.Log(id);
        m_SkillList[id].Use();

    }
}
