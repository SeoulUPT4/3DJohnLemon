using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_bgmAudioSource;
    [SerializeField]
    private AudioClip[] m_bgmClips;
    [Space(10)]

    [SerializeField]
    private AudioSource m_sfxAudioSource;
    [SerializeField]
    private AudioClip[] m_sfxAudioClips;

    void Start()
    {
        m_bgmAudioSource.clip = m_bgmClips[0];
        m_bgmAudioSource.loop = true;
        m_bgmAudioSource.Play();
    }

    public void PlayWinSound()
    {
        m_sfxAudioSource.clip = m_sfxAudioClips[0];
        m_sfxAudioSource.Play();
    }
    public void PlayGameOverSound()
    {
        m_sfxAudioSource.clip = m_sfxAudioClips[1];
        m_sfxAudioSource.Play();
    }
}
