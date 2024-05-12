using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//돌 자체를 생성하는 스크립트
public class BossRockLauncher : MonoBehaviour
{
    [SerializeField] private BossEnemy m_BossEnemy;
    [SerializeField]private GameObject m_goMissile;
    [SerializeField]private Transform[] m_transform;
    [SerializeField] private EnemySpawner m_EnemySpawner;
    private Transform m_target;
    private float m_fDamage;
    [SerializeField] private ObjectPool<BossThrowRock> m_BossRockPool;
    [SerializeField] private BossFootAttack m_bossFootAttack;
    [SerializeField] private ParticleSystem m_skeletonParticle;
    void Start()
    {
        m_BossRockPool.Initialize();
        m_fDamage = 30.0f;
    }
    public void SetTarget(Transform trans)
    {
        m_target = trans;
    }
    // Update is called once per frame
    public void FireBossRock()
    {

        foreach (Transform t in m_transform)
        {
            BossThrowRock clone;

            m_BossRockPool.GetObject(out clone);         //오브젝트 꺼내기
            clone.SetPosition(t.position);              //위치 세팅
            clone.SetInitData(m_target, m_fDamage,this);
            float speed = Random.Range(0.5f, 1.5f);
            clone.GetComponent<Rigidbody>().velocity = Vector3.up * speed;
            StartCoroutine(RemoveDelay(clone));

        }
    }
    IEnumerator RemoveDelay(BossThrowRock clone)
    {
        yield return new WaitForSeconds(5.0f);  //돌맹이가 플레이어를 따라가는 시간
        AudioManager.Instance.PlaySFX(10);
        m_BossEnemy.EndAttack();
        clone.RemoveStone();
    }

    public void Remove(BossThrowRock clone)
    {
        clone.ResetData();
        m_BossRockPool.PutInPool(clone);
        
    }

    public void FireBossSpawnSkeleton()
    {
        m_skeletonParticle.gameObject.SetActive(true);
        m_EnemySpawner.SpawnFirst();
        StartCoroutine(WaitForDisable());
    }
    IEnumerator WaitForDisable()
    {
        yield return new WaitForSeconds(5.0f);
        m_skeletonParticle.gameObject.SetActive(false);
        m_BossEnemy.EndAttack();

    }
    public void FireFootAttack()
    {
        m_bossFootAttack.gameObject.SetActive(true);
        m_bossFootAttack.Run();
    }
    public void FootAttackEnd()
    {
        m_BossEnemy.EndAttack();
    }
}
