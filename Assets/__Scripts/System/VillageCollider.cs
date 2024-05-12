using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("in");
        if (other.CompareTag("Player"))
        {
            PlayerController.Instance.m_canAttack = false; 
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Out");
        if (other.CompareTag("Player"))
        {
            PlayerController.Instance.m_canAttack = true;
        }
    }
}
