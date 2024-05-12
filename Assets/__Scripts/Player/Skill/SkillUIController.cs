using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum SkillSlotType
{
    QuickSlot,
    Skill,
}
public class SkillUIController : MonoBehaviour
{
    [SerializeField] private List<SkillSlotUI> m_skillUIs;
    [SerializeField] private TextMeshProUGUI m_SkillPointText;

    public void Start()
    {
    }
    
    public void Initialize()
    {
           
        foreach (SkillSlotUI slot in m_skillUIs)
        {

            if (slot != null)
            {
                slot.Initialize();
            }
        }
    }
    public void UpdateSkillPointUI(int pointCount)
    {
        m_SkillPointText.text = pointCount.ToString();
    }
    public void UpdateUI(Skill skill)
    {

        m_skillUIs[skill._data.m_iSkillID].SkillUIUpdate(skill._data.m_iSkillID);
    }
    public void UpdateAll()
    {
        PlayerController.Instance._PlayerSkill.UpdateSkillAll();
    }

    public void SetUI(int id)
    {

    }
}
