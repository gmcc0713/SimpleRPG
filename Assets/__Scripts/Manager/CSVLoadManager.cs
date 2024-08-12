using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//생성자에서 호출

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

        //테이블의 행들을 하나씩 저장하는 리스트
        List<string[]> tables = new List<string[]>();

        for (int i = 1; i < data.Length - 1; i++)
        {
            //,기준으로 나누기(행으로 나눠짐)
            string[] table = data[i].Split(new char[] { ',' });
            tables.Add(table);
        }


        return tables;
    }


}
