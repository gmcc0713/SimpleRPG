using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossAttackStoneRainState", menuName = "ScriptableObject/FSM State/Boss/BossAttackStoneRainState", order = 2)]
public class BossAttackStoneRainState : ScriptableObject, IState
{
    public void Enter(BaseEnemy owner)
    {
        if (owner is BossEnemy bOwner)
        {
            bOwner.RotateToTarget(RotateType.RotToTarget);
            bOwner.StopMove();
            bOwner.BossAttackSpawnSkeleton();

        }
    }
    public void Excute(BaseEnemy owner)
    {
    }
    public void Exit(BaseEnemy owner)
    {

    }
}
