using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CountingNumberText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_CountingNumberText;
    
    void Start()
    {
        m_CountingNumberText.text = "";
    }
    public void UpdateText(int targetNum)
    {
        StartCoroutine(CountText(targetNum));
    }
    IEnumerator CountText(int targetNum)
    {
        int num = 0;
        int addNum = targetNum / 100;
        while (targetNum  >= num)
        {
            num+=addNum;
            m_CountingNumberText.text = num.ToString();
            yield return new WaitForSeconds(0.01f);

        }

        num = targetNum;
        m_CountingNumberText.text = num.ToString();
    }
    
}
