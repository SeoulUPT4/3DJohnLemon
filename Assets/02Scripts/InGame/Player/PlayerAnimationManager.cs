using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] Animator m_animator;

    public void PlayWalkAnim(bool isWalking)
    {
        m_animator.SetBool("IsWalking", isWalking);
    }
}
