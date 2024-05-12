using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossFootAttack : MonoBehaviour
{
    [SerializeField] private BossFootCircle m_FootAttackCircle;
    public void Run()
    {
        m_FootAttackCircle.Run();
    }
}
