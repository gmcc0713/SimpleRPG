using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuffTimerPanel : MonoBehaviour
{
    public GameObject target;

    [SerializeField] private ObjectPool<SkillRemainTimer> m_pool;

    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        m_pool.Initialize();
    }
    public void BuffTimerOn(Skill skill)
   {
        SkillRemainTimer clone;
        m_pool.GetObject(out clone);         //오브젝트 꺼내기
        clone.transform.SetParent(transform);
        clone.SetData(skill);
        clone.StartTimer(skill);
    }
   public void BuffEnd(SkillRemainTimer timer,int num)
    {
        if (num == 0)
            num = 9;
        else
        {
            num = 10;
        }
        PlayerController.Instance._ParticleSystem.ParticleEnd(num);
        m_pool.PutInPool(timer);

    }
}
