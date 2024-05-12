using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolingObject
{
    void SetPosition(Vector3 pos);
    
}
[System.Serializable]
public class ObjectPool<T> where T : MonoBehaviour, IPoolingObject
{
    [SerializeField] T targetObject;                            //��� ������Ʈ
    [SerializeField] [Range(1, 100)] int poolingAmount = 1;     //Ǯ�� ����
    Transform containerObject;                                  //Ǯ���� ��Ҹ� �����ϴ� �θ� ������Ʈ
    Queue<T> objectPool;                                        //Ǯ���� ���
                                  //
    public void SettingParent(Transform parent = null)
    {
        containerObject = parent;
    }
    public bool Initialize()
    {
        if (!targetObject || containerObject) return false;     
        if (1 > poolingAmount) poolingAmount = 1;                            //Ǯ���Ǿ��ִ� �� 1

        System.Text.StringBuilder sb = new System.Text.StringBuilder();     //+�������δ� GC�� �߻��ϱ� ������ StringBuilder�� Append���
        sb.Append("Object Pool Container : ");
        sb.Append(targetObject.name);

        containerObject = new GameObject(sb.ToString()).transform;
        objectPool = new Queue<T>();                                            //Ǯ���� ��� �Ҵ�
        MakeAndPooling();
        return true;
    }
    bool MakeAndPooling()
    {
        if (!containerObject) return false;
        T poolObject;
        for (int i = 0; poolingAmount > i; i++)
        {
            poolObject = MonoBehaviour.Instantiate(targetObject, containerObject);
            poolObject.name = targetObject.name;
            
            poolObject.gameObject.SetActive(false);
            objectPool.Enqueue(poolObject);       
        }
        return true;
    }

    public bool GetObject(out T item)
    {
        item = null;
        if (!containerObject) return false;
        if (0 >= objectPool.Count)
        {
            if (!MakeAndPooling()) 
                return false;
        }
        item = objectPool.Dequeue();                                            //�ش� ������Ʈ ����� ���� ��������
        item.gameObject.SetActive(true);
        return true;
    }
    /// <summary> item�� ��Ȱ��ȭ ��Ű�� Pool�� �ִ´�. </summary>
    public bool PutInPool(T item)                                               //����� ������ ���ٽ� �ֱ�
    {
        if (!(item && containerObject)) return false;
        item.gameObject.SetActive(false);

        objectPool.Enqueue(item);
        item.transform.SetParent(containerObject);
        return true;
    }
    public bool Destroy()
    {
        if (!containerObject) return false;
        MonoBehaviour.Destroy(containerObject.gameObject);
        containerObject = null;
        objectPool.Clear();
        objectPool = null;
        return true;
    }

    public void ReturnBackPool()
    {
        if (containerObject)
        {
            // ��� �ڽ��� ��ȸ �Ѵ�.
            foreach (Transform child in containerObject)
            {
                if (child.gameObject.activeSelf)
                {
                    if (child.TryGetComponent(out T item)) PutInPool(item);
                }
            }
        }
    }
} // class ObjectPool

