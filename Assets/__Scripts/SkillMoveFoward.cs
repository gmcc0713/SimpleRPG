using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMoveFoward : MonoBehaviour
{
    public float speed = 5f;

    public void MoveStart()
    {
        StartCoroutine(MoveFireball());
    }
    IEnumerator MoveFireball()
    {
        while (true)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            yield return null; 
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<BaseEnemy>().GetDamage(500.0f);
            this.gameObject.SetActive(false);
        }
    }
}
