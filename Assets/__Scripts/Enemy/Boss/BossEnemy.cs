using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public enum BossAttackType
{
    None = 0,
    ThrowRock,
    FootAttack,
    RainRock,
    Count,
};

public class BossEnemy : BaseEnemy
{
   [SerializeField] BossRockLauncher m_bossRockLauncher;
   [SerializeField] Slider m_bossHealth;
    private bool IsSpawn = false;
    private void Start()
    {
        
        m_bossRockLauncher = GetComponentInChildren<BossRockLauncher>();
        m_bossRockLauncher.SetTarget(target.transform);
        enemyHealthBar = DungeonManager.Instance.GetHealthbar();
        enemyHealthBar.gameObject.SetActive(true);
        enemyHealthBar.value = 1;
        AudioManager.Instance.PlaySFX(12);
        IsSpawn = false;
    }
    IEnumerator BossAttackDelay()
    {
        yield return new WaitForSeconds(3f);
        fsm.ChangeState(stateData.IdleState);

    }
    public void EndAttack()
    {
        animator.SetBool("IsAttack", false);
        animator.SetInteger("AttackType", 0);

        StartCoroutine(BossAttackDelay());
    }
    public override void AttackTarget()
    {
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsAttack", true);
        int attackType;
        if (IsSpawn)
        {
            attackType = Random.Range(1, (int)BossAttackType.Count - 1);
        }
        else
        {
            attackType = Random.Range(1, (int)BossAttackType.Count);
        }

        if (attackType == (int)BossAttackType.RainRock)
            IsSpawn = true;
        animator.SetInteger("AttackType", attackType);

        switch ((BossAttackType)attackType)
        {
            case BossAttackType.ThrowRock:
                fsm.ChangeState(stateData.BossStoneThrowState);
                break;
            case BossAttackType.FootAttack:
                fsm.ChangeState(stateData.BossFootAttackState);
                break;
            case BossAttackType.RainRock: 
                fsm.ChangeState(stateData.BossStoneRainState);
                break;
        }
    }
    public void RockLauncherRun()
    {
        m_bossRockLauncher.FireBossRock();
    }
    public void BossAttackSpawnSkeleton()
    {
        m_bossRockLauncher.FireBossSpawnSkeleton();
    }
    public void BossAttackFootAttack()
    {

        m_bossRockLauncher.FireFootAttack();
    }
    // Update is called once per frame
}
