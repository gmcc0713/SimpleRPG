using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossAttackFootAttackState", menuName = "ScriptableObject/FSM State/Boss/BossAttackFootAttackState", order = 1)]
public class BossAttackFootAttackState : ScriptableObject, IState
{
    public void Enter(BaseEnemy owner)
    {
        if (owner is BossEnemy bOwner)
        {
            bOwner.RotateToTarget(RotateType.RotToTarget);
            bOwner.StopMove();
            bOwner.BossAttackFootAttack();
        }
    }
    public void Excute(BaseEnemy owner)
    {
        //�� ������ ���� �ٲ�� ���� ������
    }
    public void Exit(BaseEnemy owner)
    {

    }
}
