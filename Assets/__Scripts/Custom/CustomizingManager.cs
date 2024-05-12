using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CustomizingManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PlayerCustom m_playerCustom;
    [SerializeField] private int []m_curIdx;
    [SerializeField] private int []m_maxIdx;

    [SerializeField] private TextMeshProUGUI []m_textIdx;
    [SerializeField] private TMP_InputField m_NickNameInput;
    [SerializeField] private TextMeshProUGUI m_NicknameAnnounce;
    [SerializeField] private Button m_NicknameBtn;
    [SerializeField] private PlayerData m_playerData;
    private bool m_bCanMake;
    private void Start()
    {
        Initialize();
    }
    void Initialize()
    {
        PlayerController.Instance.SetCanAct(false);
    }
    public void ClickRightBtn(int type)
    {
        m_curIdx[type]++;
        if (m_maxIdx[type]-1 < m_curIdx[type])
        {
            m_curIdx[type] = 0;
        }
        m_playerCustom.ChangePlayerCustom(type, m_curIdx[type]);
        TextChange(type);
    }
    public void ClickLeftBtn(int type)
    {
        m_curIdx[type]--;
        if (0 > m_curIdx[type])
        {
            m_curIdx[type] = m_maxIdx[type] - 1;
        }
        Debug.Log(m_curIdx[type]);
        m_playerCustom.ChangePlayerCustom(type, m_curIdx[type]);
        TextChange(type);
    }
    public void TextChange(int type)
    {
        string s = "";
        switch (type)
        {
            case 0:
                s = "��� ";
                break;
            case 1:
                s = "�� ";
                break;
            case 2:
                s = "�� ";
                break;
        }
        m_textIdx[type].text = s + (m_curIdx[type] +1);
        
    }
    public void NicknameCheck()
    {
        if (m_NickNameInput.text.Length > 4)
        {
            m_NicknameAnnounce.text = "�̸��� �ʹ� ��ϴ�.";
            m_NicknameAnnounce.gameObject.SetActive(true);
        }
        else
            m_NicknameAnnounce.gameObject.SetActive(false);
    }
    public void NicknameInputClear()
    {
        m_NickNameInput.interactable = false;
        m_NicknameBtn.gameObject.SetActive(false);
        m_bCanMake = true;
        
    }
    public void NicknameBtnClickCheck()
    {
        if (m_NickNameInput.text.Length <= 4 && m_NickNameInput.text.Length > 0)
        {
            NicknameInputClear();
        }
        else
        {
            m_NicknameAnnounce.text = "�ٽ� �Է��ϼ���.";
        }
        

    }
    public IEnumerator WaitForDisable()
    {
        yield return new WaitForSeconds(1.5f);
        m_NicknameAnnounce.gameObject.SetActive(false);
    }
    public void MakePlayer(GameObject panel)
    {
        if (m_bCanMake)
        {
            panel.SetActive(false);
            PlayerController.Instance.SetCanAct(true);
            PlayerController.Instance.SetPlayerDataByCustomizing(m_NickNameInput.text);
        }
        else
        {
            m_NicknameAnnounce.gameObject.SetActive(true);
            m_NicknameAnnounce.text = "�̸��� �Է��ϼ���.";

        }
    }
}
