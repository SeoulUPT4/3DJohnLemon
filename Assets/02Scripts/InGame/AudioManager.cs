using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField]
    private AudioSource m_bgmAudioSource;
    [SerializeField]
    private AudioClip[] m_bgmClips;
    [Space(10)]

    [SerializeField]
    private AudioSource m_sfxAudioSource;
    [SerializeField]
    private AudioSource m_sfxAudioSource2;
    [Header("[ Skill ]")]
    [SerializeField]
    private AudioClip m_flashLightClip;
    [SerializeField]
    private AudioClip m_mapScanClip;
    [Space(10)]

    [Header("[ GamePlay ]")]
    [SerializeField]
    private AudioClip m_winClip;
    [SerializeField]
    private AudioClip m_gameOverClip;
    [Space(10)]

    [Header("[ InteractionComponent ]")]
    [SerializeField]
    private AudioClip m_pickUpKeyClip;
    [SerializeField]
    private AudioClip m_lockedDoorClip;
    [SerializeField]
    private AudioClip[] m_openedDoorClip;
    [SerializeField]
    private AudioClip m_closedDoorClip;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        m_bgmAudioSource.clip = m_bgmClips[0];
        m_bgmAudioSource.loop = true;
        m_bgmAudioSource.Play();
    }

    // 재사용성은 추후 추가적인 사운드 처리 끝난후 한번에 정리하여 통합
    public void PlayFlashLightSound()
    {
        m_sfxAudioSource.clip = m_flashLightClip;
        m_sfxAudioSource.Play();
    }
    public void PlayMapScanSound()
    {
        m_sfxAudioSource.clip = m_mapScanClip;
        m_sfxAudioSource.Play();
    }
    public void PlayWinSound()
    {
        m_sfxAudioSource.clip = m_winClip;
        if (!m_sfxAudioSource.isPlaying)
            m_sfxAudioSource.Play();
    }
    public void PlayGameOverSound()
    {
        m_sfxAudioSource.clip = m_gameOverClip;
        if (!m_sfxAudioSource.isPlaying)
            m_sfxAudioSource.Play();
    }

    public void PlayPickUpSound()
    {
        m_sfxAudioSource.clip = m_pickUpKeyClip;
        if(!m_sfxAudioSource.isPlaying)
            m_sfxAudioSource.Play();
    }
    public void PlayLockedDoorSound()
    {
        m_sfxAudioSource.clip = m_lockedDoorClip;
        if (!m_sfxAudioSource.isPlaying)
            m_sfxAudioSource.Play();
    }

    public void PickUpKeySound()
    {
        m_sfxAudioSource.clip = m_pickUpKeyClip;
        if (!m_sfxAudioSource.isPlaying)
            m_sfxAudioSource.Play();
    }
    public void PlayOpenedDoor()
    {
        // Door UnLock Open
        m_sfxAudioSource.clip = m_openedDoorClip[0];
        if (!m_sfxAudioSource.isPlaying)
            m_sfxAudioSource.Play();

        // Door Opened
        m_sfxAudioSource2.clip = m_openedDoorClip[1];
        StartCoroutine(OpenedDoorCoroutine(m_openedDoorClip[0].length * 0.5f));
    }
    IEnumerator OpenedDoorCoroutine(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (!m_sfxAudioSource.isPlaying)
            m_sfxAudioSource2.Play();
    }
    
    public void PlayClosedDoor()
    {
        m_sfxAudioSource.clip = m_closedDoorClip;
        if (!m_sfxAudioSource.isPlaying)
            m_sfxAudioSource.Play();
    }
}
