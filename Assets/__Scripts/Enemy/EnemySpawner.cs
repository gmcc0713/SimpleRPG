using System.Collections;
using System.Collections.Generic;

using UnityEngine;



public class EnemySpawner : MonoBehaviour
{
    public GameObject target;

    [SerializeField] private ObjectPool<BaseEnemy> enemyPool;
    public float spawnDelay;
    private float currentTime;
    public bool spawning = false;

    [SerializeField] private bool m_bIsSpawnOnce;

    [SerializeField] private List<Transform> spawnPos = new List<Transform>();

    [SerializeField] private int maxSpawnCount = 4;
    public int curSpawnCount = 0;
    void Start()
    {
        enemyPool.Initialize();
        currentTime = spawnDelay;
    }


    public void SpawnFirst()
    {
        for(int i = curSpawnCount; i<maxSpawnCount;i++)
        {
            Spawn(spawnPos[i].transform.position);
        }
    }

   public void Spawn(Vector3 spawnPosition)
    {
        BaseEnemy clone;
        enemyPool.GetObject(out clone);         //������Ʈ ������

        clone.SetSpawner(this);                 //������ enemy data����
        clone.SetPosition(spawnPosition);       //��ġ ����

        curSpawnCount++;


        clone.ResetData();
        EnemyManager.Instance.EnemyInit(clone);

        clone.Run();                                        //������Ʈ�۵���Ű��
                                                            //���� �ڵ�

    }
    public void DieEnemy(BaseEnemy enemy)
    {
        curSpawnCount--;
        if(!m_bIsSpawnOnce)
        {
            StartCoroutine(RespawnEnemy(enemy._defaultPos));
        }
        enemyPool.PutInPool(enemy);

    }
    public IEnumerator RespawnEnemy(Vector3 pos)
    {

        yield return new WaitForSeconds(1f);
        if (CanEnemySpawnCountCheck())
        {
            Spawn(pos);
        }
        
    }

    public bool CanEnemySpawnCountCheck()          //�����Ҽ� �ִ� ���� Ȯ��
    {
        
        if ( maxSpawnCount > curSpawnCount)
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnFirst();
            spawning = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        spawning = false;
        Debug.Log("���� ���� ����");
    }
}
