using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DungeonReward : MonoBehaviour
{
    [SerializeField] private CountingNumberText m_gold;
    [SerializeField] private CountingNumberText m_exp;
    [SerializeField] private Transform m_parent;
    [SerializeField] private ObjectPool<RewardSlot> pool;
    
    // Start is called before the first frame update
    private void Start()
    {
        pool.Initialize();
        pool.SettingParent(m_parent);
    }
    public void RewardSet(int gold,int exp,int[] itemNums)
    {



        m_gold.UpdateText(gold);
        m_exp.UpdateText(exp);
        foreach(var num in itemNums)
        {
            Debug.Log(num);
            RewardSlot clone;
            pool.GetObject(out clone);         //오브젝트 꺼내기
            clone.SetImage(ItemDataManager.Instance.FindItemImage(num));
        }
    }
}
