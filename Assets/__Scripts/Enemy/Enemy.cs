using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public enum RotateType { RotToTarget,RotToSpawnPos}
public class Enemy :BaseEnemy
{

    public override void AttackTarget()
    {
        animator.SetBool("IsAttack", false);
        StopMove();
        RotateToTarget(RotateType.RotToTarget);


        if (DistanceToTarget() > m_cEnemyData._attackRange)
        {
            animator.SetBool("IsAttack", false);
            fsm.ChangeState(stateData.MoveState);
        }
        if(m_bCanAttack)
        {
            animator.SetBool("IsAttack", true);
            AudioManager.Instance.PlaySFX(7 + (int)m_cEnemyData._type);
            target.GetComponent<PlayerController>().GetDamaged(m_cEnemyData._atkPower);
            StartCoroutine(AttackDelay());
        }
    }
   
}
