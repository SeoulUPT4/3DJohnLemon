using System.Collections;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] Animator m_animator;

    public void PlayWalkAnime(bool isWalking)
    {
        m_animator.SetBool("IsWalking", isWalking);
    }
    public void PlayFlashAnime(float time)
    {
        m_animator.SetBool("IsFlash", true);
        StartCoroutine(PlayAnimeTimeCoroutine(time, SkillType.Flash));
    }

    public void PlayMapScanAnime(float time)
    {
        m_animator.SetBool("IsMapScan", true);
        StartCoroutine(PlayAnimeTimeCoroutine(time, SkillType.MapScan));
    }

    private IEnumerator PlayAnimeTimeCoroutine(float time, SkillType skillType)
    {
        yield return new WaitForSeconds(time);
        switch(skillType)
        {
            case SkillType.Flash:
                m_animator.SetBool("IsFlash", false);
                break;
            case SkillType.MapScan:
                m_animator.SetBool("IsMapScan", false);
                break;
        }
        
    }

}
