using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationEventSystem : MonoBehaviour
{
   [SerializeField] private GameObject m_AttackCollider;
    [SerializeField] private ParticleSystem[] m_slashparticle;
    [SerializeField] private Transform[] m_spawnPos;

    public void Attack(int num)
    {
        m_AttackCollider.SetActive(true);
        m_slashparticle[num].gameObject.SetActive(true);
        AudioManager.Instance.PlaySFX(14);
    }

    public void AttackEnd()
    {
        m_AttackCollider.SetActive(false); 

    }
    public void ParticlePlayDuringTime(int num,float timer)
    {
        StartCoroutine(PlayDuringTime(num,timer));
    }
    private IEnumerator PlayDuringTime(int num,float timer)
    {
        m_slashparticle[num].transform.position = m_spawnPos[num].position;
        m_slashparticle[num].gameObject.SetActive(true);
        yield return new WaitForSeconds(timer);
        m_slashparticle[num].gameObject.SetActive(false);
    }
    public void ParticlePlay(int num)
    {
        if (m_slashparticle[num].gameObject.activeSelf)
        {
            m_slashparticle[num].gameObject.SetActive(false);
        }
        m_slashparticle[num].transform.position = m_spawnPos[num].position;
        m_slashparticle[num].gameObject.SetActive(true);
    }
    public void ParticleEnd(int num)
    {
        Debug.Log("end");
        if(num == 10)
        {
            PlayerController.Instance.SetCantDamaged(false);
        }
        m_slashparticle[num].gameObject.SetActive(false);
    }
    public void SkllMoveStart(int num)
    {
        m_slashparticle[num].gameObject.SetActive(true);
    }
    public void SetPlayerCanAct()
    {
        Debug.Log("SetAct");
        PlayerController.Instance.SetCanAct(true);

    }
    public void TeleportAnimation()
    {
        Vector3 pos = PlayerController.Instance.transform.position;
        PlayerController.Instance.transform.position = pos + Vector3.forward * 10f;
    }
}
