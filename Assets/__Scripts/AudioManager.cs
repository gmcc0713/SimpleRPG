using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    void Awake()
    {
        if (null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    [SerializeField] private AudioClip[] m_BGMClips;
    [SerializeField] private AudioClip[] m_SFXClips;

    [SerializeField] private List<AudioSource> m_audioSourceSFXs;
    [SerializeField] private AudioSource m_audioSourceBGM;

    private float sfxVolum = 0.5f;
    void Start()
    {
        m_audioSourceSFXs = new List<AudioSource>();
        m_audioSourceBGM.volume = 0.5f;
        ChangeBGM(0);
    }

    // Update is called once per frame
    public void ChangeBGMVolume(float num)
    {
        m_audioSourceBGM.volume = num;
    }
    public void ChangeSFXVolume(float num)
    {
        sfxVolum = num;
    }
    public void ChangeBGM(int num)
    {
        m_audioSourceBGM.clip = m_BGMClips[num];
        m_audioSourceBGM.Play();
    }
    public void PlaySFX(int num,float delayTime = 0)
    {
        StartCoroutine(SoundDelay(num, delayTime));

            
    }
    IEnumerator SoundDelay(int num, float delayTime = 0)
    {
        yield return new WaitForSeconds(delayTime);
        foreach (AudioSource source in m_audioSourceSFXs)
        {
            if (source == m_audioSourceBGM)
                continue;
            if (source.clip == null)
            {
                source.volume = sfxVolum;
                source.clip = m_SFXClips[num];
                source.PlayOneShot(source.clip);
                source.clip = null;
                yield break;
            }
        }
        AudioSource newsource = gameObject.AddComponent<AudioSource>();
        m_audioSourceSFXs.Add(newsource);
        
        newsource.clip = m_SFXClips[num];
        newsource.volume = sfxVolum;
        newsource.PlayOneShot(newsource.clip);
        newsource.clip = null;
        yield break;
    }
    
}
