using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�����ڿ��� ȣ��

public class CSVLoadManager : MonoBehaviour
{
    public static CSVLoadManager Instance { get; private set; }
    public TextAsset m_csvFile;

    void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    public List<string[]> Load(string path)
    {
        m_csvFile = (TextAsset)Resources.Load(path);
        string[] data = m_csvFile.text.Split(new char[] { '\n' });

        //���̺��� ����� �ϳ��� �����ϴ� ����Ʈ
        List<string[]> tables = new List<string[]>();

        for (int i = 1; i < data.Length - 1; i++)
        {
            //,�������� ������(������ ������)
            string[] table = data[i].Split(new char[] { ',' });
            tables.Add(table);
        }


        return tables;
    }


}
