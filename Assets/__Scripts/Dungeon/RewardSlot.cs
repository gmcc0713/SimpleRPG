using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardSlot :MonoBehaviour, IPoolingObject
{
    [SerializeField] private Image m_itemImage;
    public void SetPosition(Vector3 pos) { }
   public void SetImage(Sprite image)
    {
        m_itemImage.sprite = image;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
