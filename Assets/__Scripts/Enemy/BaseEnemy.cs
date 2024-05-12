using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class BaseEnemy : MonoBehaviour, IPoolingObject
{
    [SerializeField] protected int EnemyID;
    protected EnemyData m_cEnemyData;
    [SerializeField] protected EnemyDropItemDatas m_eEnemyDropItems;

    public EnemyData _EnemyData => m_cEnemyData;

    [SerializeField] protected NavMeshAgent agent;
    protected EnemyFSM fsm;
    protected StateData stateData = null;
    protected EnemySpawner enemySpawner;
    protected Animator animator;
    protected float curHealth;
    [SerializeField] protected Slider enemyHealthBar;

    [SerializeField] protected bool isMoving;
    [SerializeField] protected Transform target;
    protected Vector3 defaultPos;
    public Vector3 _defaultPos => defaultPos;
    public Animator _animator => animator;
    [SerializeField] protected bool m_bIsDamaged = false;
    protected bool isDie = false;
    protected bool m_bCanAttack = true;
    private void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
        m_bIsDamaged = false;

    }
    public void ResetData()
    {

        if (fsm != null)
        {
            fsm.ChangeState(stateData.IdleState);
        }

        if (m_cEnemyData == null)
        {
            m_cEnemyData = EnemyManager.Instance.GetEnemyData(EnemyID);
        }
        curHealth = m_cEnemyData._maxHealth;
        if(enemyHealthBar!=null)
            enemyHealthBar.value = 1;
        isDie = false;

    }
    public void SetPosition(Vector3 pos)
    {
        agent.gameObject.SetActive(false);
        transform.position = pos;
        agent.gameObject.SetActive(true);
    }
    public void SetSpawner(EnemySpawner spawner)           //ó�� ����
    {
        enemySpawner = spawner;

    }
    // Update is called once per frame
    public bool SetData(StateData data, EnemyData enemyData)            //���� �ʱ⿡ �ѹ� ������ ����
    {
        stateData = data;

        if (null == enemyData)
        {
            return false;
        }
        if (null == fsm)
        {
            fsm = new EnemyFSM(this);
        }
        if (!fsm.SetCurrState(stateData.IdleState))
        {
            return false;
        }

        isDie = false;
        gameObject.SetActive(true);

        defaultPos = transform.position;

        ResetData();

        m_eEnemyDropItems = EnemyManager.Instance.GetEnemyDropItemDatas(m_cEnemyData._dropItemID);
        return true;
    }

    public void Run()                           //update�ڷ�ƾ ����
    {
        gameObject.SetActive(true);
        StartCoroutine(OnUpdate());
    }
    IEnumerator OnUpdate()
    {
        target = GameObject.FindWithTag("Player").transform;
        while (true)
        {
            fsm.Update();

            yield return new WaitForSeconds(0.04f);
        }
    }

    public void SearchTarget()      // Ÿ���� ���� �ȿ� �ִ��� Ȯ��
    {
        if (DistanceToTarget() > 0 && DistanceToTarget() <= m_cEnemyData._searchRange)
        {
            fsm.ChangeState(stateData.MoveState);
        }
    }
    public float DistanceToTarget()     //Ÿ�ٰ��� �Ÿ� 
    {
        if (!target) return -1;
        return Vector3.Distance(target.position, transform.position);
    }
    public void MoveToTarget()          //Ÿ�ٿ��Է� �̵�
    {
        animator.SetBool("IsMoving", true);
        if (DistanceToTarget() < m_cEnemyData._attackRange)       //���ݻ�Ÿ� �ȿ� Ÿ���� ������
        {
            fsm.ChangeState(stateData.AttackState);
            return;
        }
        else if (DistanceToTarget() < m_cEnemyData._searchRange)  //Ž����Ÿ� �ȿ� Ÿ���� ������
        {
            agent.isStopped = false;

            agent.SetDestination(target.position);
            return;
        }
        fsm.ChangeState(stateData.ReturnPosState);  //Ž����Ÿ��� �������
    }
    public void RotateToTarget(RotateType type)     //Ÿ������ ȸ��
    {
        Vector3 tar = Vector3.zero;
        switch (type)
        {
            case RotateType.RotToTarget:
                tar = target.transform.position;
                break;
            case RotateType.RotToSpawnPos:
                tar = defaultPos;
                break;
        }
        Vector3 dir = Vector3.zero;
        if (tar == defaultPos)
            dir = tar;
        else
            dir = tar - transform.position;
        dir.y = 0f;

        Quaternion rot = Quaternion.LookRotation(dir.normalized);

        transform.rotation = rot;
    }
    public void StopMove()      //������ ���߱�
    {
        agent.isStopped = true;
        agent.ResetPath();
    }
    public void StartMove()
    {
        agent.isStopped = false;
    }
    public void ReturnDefaultPos()      //������ �ڸ��� ���ư���
    {
        if (DistanceToTarget() > 0 && DistanceToTarget() <= m_cEnemyData._searchRange)    //Ÿ���� �����ȿ� ��������
        {
            fsm.ChangeState(stateData.MoveState);
            return;
        }
        else if (agent.remainingDistance > 1)       //������ �ڸ��� ���ư��� ���϶�
        {
            agent.SetDestination(defaultPos);
            return;
        }
        fsm.ChangeState(stateData.IdleState);


    }
    public virtual void AttackTarget()
    {
        
    }
    public IEnumerator AttackDelay()
    {
        m_bCanAttack = false;
        yield return new WaitForSeconds(m_cEnemyData._attackDuration);
        m_bCanAttack = true;
    }
    public void SetDieState()
    {
        isDie = true;
        animator.SetBool("IsDie", true);
        QuestManager.Instance.NotifyQuestUpdate(Quest_Type.Type_Kill, (int)m_cEnemyData._type, 1);
        if (DungeonManager.Instance)
        {
            if(m_cEnemyData._type == EnemyType.Golam)
            {
                DungeonManager.Instance.DungeonClear();
            }
            DungeonManager.Instance.DieMonster();
            
        }
        DropItem();
    }
    public void Die()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f && animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            fsm.ChangeState(stateData.IdleState);
            enemySpawner.DieEnemy(this);
        }
    }
    IEnumerator WaitForNextAttacked()
    {
        m_bCanAttack = true;
        yield return new WaitForSeconds(0.3f);
        m_bCanAttack = false;
    }
    public void GetDamage(float damage)
    {
        curHealth -= damage;
        enemyHealthBar.value = curHealth / m_cEnemyData._maxHealth;

        StartCoroutine(WaitForNextAttacked());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttackCollider") && !m_bIsDamaged)
        {
            curHealth -= other.GetComponent<AttackCollider>().CalculateDamage();

            enemyHealthBar.value = curHealth / m_cEnemyData._maxHealth;
            StartCoroutine(WaitForNextAttacked());
        }
        if (other.CompareTag("SkillCollider") && !m_bIsDamaged)
        {
            curHealth -= other.GetComponent<ParticleCollider>().GetDamage();
            enemyHealthBar.value = curHealth / m_cEnemyData._maxHealth;

            StartCoroutine(WaitForNextAttacked());
        }
        if (curHealth <= 0 && isDie == false)
        {
            fsm.ChangeState(stateData.DieState);
        }
    }
    public void DropItem()
    {
        for (int i = 0; i < m_eEnemyDropItems.dropItems.Count; i++)
        {
            int rand = Random.RandomRange(0, 100);
            if (rand < m_eEnemyDropItems.dropItems[i].dropPercent)
            {
                PlayerController.Instance._Inventory.GetItem(m_eEnemyDropItems.dropItems[i].itemID);
                //Inventory.Instance.GetItem(0);

            }
        }
        PlayerController.Instance._Inventory.AddGold(m_eEnemyDropItems.gold);
        PlayerController.Instance.AddExp(m_eEnemyDropItems.exp);

    }

}
