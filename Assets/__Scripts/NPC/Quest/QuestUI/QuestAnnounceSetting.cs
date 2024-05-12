using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestAnnounceSetting : MonoBehaviour
{
    [SerializeField] private ObjectPool<AnnounceText> pool;

    private void Start()
    {
        initialize();
    }
    
    private void initialize()
    {
        pool.Initialize();
    }
    public void AddQuestAnnounceText(string text)
    {
        AnnounceText announceText;
        pool.GetObject(out announceText);
        announceText.SetText(text);
        announceText.Initialize(this);
        announceText.transform.SetParent(transform);
    }
    public void PutInPoolAnnounceText(AnnounceText text)
    {
        pool.PutInPool(text);
    }
}
//Ǯ���� content�ڽĿ��� �۵��ǰ� ����
//���ӿ�����Ʈ ��ü�� Ǯ���ϵ��� ����