using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BossSpawnCutScene : MonoBehaviour
{
    [SerializeField]PlayableDirector m_PD;
    [SerializeField]PlayableDirector m_Clear;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            m_PD.Play();
        }
    }

}
