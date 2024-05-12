using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SkillRemainTimer : MonoBehaviour,IPoolingObject
{
    [SerializeField] private TextMeshProUGUI m_timer;
    [SerializeField] private Image m_Fill;
    [SerializeField] private Image m_SkillImage;
    private float m_fmaxActiveTime;
    BuffTimerPanel parent;

    public void SetPosition(Vector3 pos){ }
    public void SetData(Skill skill)
    {
        m_fmaxActiveTime = skill._data.m_iActiveTime;
        m_timer.text = m_fmaxActiveTime.ToString();
        m_SkillImage.sprite = skill._data.m_SkillImage;
        m_Fill.fillAmount = 1;
        parent = transform.GetComponentInParent<BuffTimerPanel>();

    }

    public void StartTimer(Skill skill)
    {
        StartCoroutine(BuffTimerStart());
        StartCoroutine(Cooltime(skill._data.m_iSkillID));

    }
    private IEnumerator BuffTimerStart()
    {
        for (int i = 0; i < m_fmaxActiveTime; i++)
        {
            m_timer.text = (m_fmaxActiveTime - i).ToString();
            yield return new WaitForSeconds(1.0f);
        }
    }
    public IEnumerator Cooltime(int id)
    {
        while (m_Fill.fillAmount > 0)
        {
            m_Fill.fillAmount -= 1 * Time.smoothDeltaTime / m_fmaxActiveTime;
            yield return null;
        }
        parent.BuffEnd(this, id);
        yield break;
    }
}
