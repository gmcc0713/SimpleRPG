using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSlotRightBtn : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private SkillSlotUI m_slot;
    [SerializeField] private GameObject m_EquipmentBtn;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(PlayerController.Instance._PlayerSkill.GetSkillByIndex(m_slot._SkillID)._skillType == SkillType.Active)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                Vector3 pos = eventData.position;
                pos += new Vector3(100, 0, 0);
                m_EquipmentBtn.transform.position = pos;

                m_EquipmentBtn.SetActive(true);
                QuickSlotManager.Instance.SetSkillEquipmentID(m_slot._SkillID);
                Debug.Log("Right");
            }
        }
    }
    public void SelectQuickSlotSetColor()
    {
        //QuickSlotManager.Instance.
    }

}
