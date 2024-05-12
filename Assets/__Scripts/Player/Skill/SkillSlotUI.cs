using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class SkillSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image m_LockedImage;
    [SerializeField] private Button m_btn;
    [SerializeField] private SkillToolTip m_skillToolTip;

    [SerializeField] private int m_slotNum;
    [SerializeField] private int skillID;

    public Image _SkillImage => m_btn.image;
    public int _SkillID=> skillID;

    public void Initialize()
    {

        m_btn = GetComponentInChildren<Button>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        
        m_skillToolTip.SetPosition(transform.position + new Vector3(200,-100, 0));
        m_skillToolTip.gameObject.SetActive(true);
        SkillData s = PlayerController.Instance._PlayerSkill.GetSkillByIndex(skillID)._data;
        Debug.Log(s.m_sSkillName);
        m_skillToolTip.SetData(s.m_sSkillName,s.m_sSkillDescription, s.m_fMP.ToString(), s.m_fCoolTime.ToString());

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(m_skillToolTip.enabled)
            m_skillToolTip.gameObject.SetActive(false);
    }


    //스킬의 UI 업데이트
    public void SkillUIUpdate(int id)
    { 
        Skill skill = PlayerController.Instance._PlayerSkill.GetSkillByIndex(id);
        Color c = m_btn.image.color;
        switch (skill._skillType)
        {
            case SkillType.Active:
                c.a = 1f;
                m_btn.image.color = c;
                m_LockedImage.gameObject.SetActive(false);
                break;
            case SkillType.Locked:
                c.a = 0.1f;
                m_btn.image.color = c;
                m_LockedImage.gameObject.SetActive(true);
                break;
            case SkillType.UnLocked:
                c.a = 0.1f;
                m_btn.image.color = c;
                m_LockedImage.gameObject.SetActive(false);
                break;
        }
        m_btn.image.sprite = skill._data.m_SkillImage;
    }
    public void LearnSkill()
    {
        PlayerController.Instance._PlayerSkill.SkillLearn(m_slotNum);
    }
  

}
