using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SkillToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_nameText;
    [SerializeField] private TextMeshProUGUI m_descriptionText;
    [SerializeField] private TextMeshProUGUI m_SkillMP;
    [SerializeField] private TextMeshProUGUI m_SkillCooltime;
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    public void SetData(string name,string descript,string mp,string cooltime)
    {
        m_nameText.text = name;
        m_descriptionText.text = descript;
        m_SkillMP.text = mp;
        m_SkillCooltime.text = cooltime;
    }


}
