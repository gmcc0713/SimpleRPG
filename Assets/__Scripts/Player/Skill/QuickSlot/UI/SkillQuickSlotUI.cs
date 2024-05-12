using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillQuickSlotUI : MonoBehaviour
{

    [SerializeField] protected Sprite m_baseImage;
    [SerializeField] protected Image m_skillImage;

    [SerializeField] private Image m_FillImage;
    [SerializeField] private TextMeshProUGUI m_coolText;
    [SerializeField] private Button m_SkillEquipmentButton;

    private bool m_bIsEmpty = true;

    private int m_iSkillID;

    public void QuickSlotSet(int id)
    {
        m_iSkillID = id;
    }
    public void SetQuickSlotDetailUI(float fillAmount, string curCoolDown)
    {
        m_FillImage.fillAmount = fillAmount;
        m_coolText.text = curCoolDown;
    }
    public void SetCoolDownText(string curCoolDown)
    {
        m_coolText.text = curCoolDown;
    }
    public void SetCoolDownFillImage(float fillAmount)
    {
        m_FillImage.fillAmount = fillAmount;
    }
    public void SetUI(Sprite  sprite)
    {
        //이미지 장착
        m_skillImage.sprite = sprite;
        m_bIsEmpty = false;
    }
    public void SetColor(bool b)
    {
        m_SkillEquipmentButton.gameObject.SetActive(true);
        Color c;
        if (b)
        {
            c = Color.red;
            c.a = 0.4f;
            m_SkillEquipmentButton.image.color = c;
            return;
        }
        c = Color.green;
        c.a = 0.4f;
        m_SkillEquipmentButton.image.color = c;
    }
    public void SelectBtnOff()
    {
        m_SkillEquipmentButton.gameObject.SetActive(false);
    }
    public bool CanSelect()   //스킬 버튼을 눌렀을때
    {
            return m_SkillEquipmentButton.image.color.r == 1;

    }
    public void SetEmpty()
    {
        m_bIsEmpty = true;
        m_skillImage.sprite = m_baseImage;
    }
}
