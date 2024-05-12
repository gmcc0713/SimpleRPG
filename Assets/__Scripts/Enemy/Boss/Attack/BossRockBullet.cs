using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossRockBullet : MonoBehaviour
{
    protected float m_fDamage;

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            RemoveStone();
        }
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX(10);
            PlayerController.Instance.GetDamaged(m_fDamage);
            RemoveStone();
        }
    }
    public virtual void RemoveStone() { } 
}
