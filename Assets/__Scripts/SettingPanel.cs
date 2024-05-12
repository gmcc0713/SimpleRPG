using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.Rendering.DebugUI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private Slider m_BGMSoundSlider;
    [SerializeField] private Slider m_SFXSoundSlider;
    [SerializeField] private Slider m_MouseSenSlider;
    [SerializeField] private CinemachineFreeLook m_tpsCam;
    bool[] mute;
    private void Start()
    {
        mute = new bool[2];
        mute[0] = false;
        mute[1] = false;
    }
    public void MuteBGMBtn()
    {
        if(mute[0])
        {
            m_BGMSoundSlider.interactable = true;
            AudioManager.Instance.ChangeBGMVolume(m_BGMSoundSlider.value);
        }
        else
        {
            
            m_BGMSoundSlider.interactable = false;
            AudioManager.Instance.ChangeBGMVolume(0);
        }
        mute[0]= !mute[0];
    }
    public void MuteSFXBtn()
    {
        if (mute[1])
        {
            m_SFXSoundSlider.interactable = true;
            AudioManager.Instance.ChangeSFXVolume(m_SFXSoundSlider.value);
        }
        else
        {
            m_SFXSoundSlider.interactable = false;
            AudioManager.Instance.ChangeSFXVolume(0);
        }
        mute[1]= !mute[1];
    }
    public void ChangeBGMValue()
    {
        AudioManager.Instance.ChangeBGMVolume(m_BGMSoundSlider.value);
    }
    public void ChangeSFXValue()
    {
        AudioManager.Instance.ChangeBGMVolume(m_SFXSoundSlider.value);
    }
    public void ChangeMSValue()
    {
        m_tpsCam.m_XAxis.m_MaxSpeed = (m_MouseSenSlider.value) * 1001.0f;
    }
}
