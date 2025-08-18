using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField]
    private UIManager m_uiManager;
    [SerializeField]
    private AudioManager m_audioManager;

    [Header( "[ Dead Config ]" )]
    [SerializeField] private float m_fadeDuration = 3;

    /*public GameObject player;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    public CanvasGroup caughtBackgroundImageCanvasGroup;

    public AudioSource exitAudio;
    public AudioSource caughtAudio;

    [SerializeField] string creditSceneName;

    bool m_IsPlayerAtExit;
    bool m_IsPlayerCaught;
    float m_Timer;
    bool m_HasAudioPlayed;*/

    private void Awake()
    {
        Instance = this;
    }

    /*void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
        }
    }*/

    void Update()
    {
        /*if (m_IsPlayerAtExit)
        {
            EndLevel(exitBackgroundImageCanvasGroup, false, exitAudio);
        }
        else if (m_IsPlayerCaught)
        {
            EndLevel(caughtBackgroundImageCanvasGroup, true, caughtAudio);
        }*/
    }
    
    public void PlayerDead()
    {
        // 사운드 효과
        m_audioManager.PlayGameOverSound();

        // DeadUI FadeIn
        m_uiManager.DeadUIFade(m_fadeDuration);

        // 현재 씬 재 로드
        Invoke("DelaySceneLoad", m_fadeDuration + 2f);
    }
    private void DelaySceneLoad()
    {
        SceneLoader.ReLoadCurrentScene();
    }

    /*public void CaughtPlayer()
    {
        m_IsPlayerCaught = true;
    }

    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)
    {
        if (!m_HasAudioPlayed)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }
    }*/
}
