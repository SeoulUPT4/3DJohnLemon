using UnityEngine;

public class Lever : InteractionComponent
{
    public Animator animator;
    public bool m_isOpened = false;
    public override bool Interact(PlayerInventory inventory)
    {
        m_isOpened = !m_isOpened;
        animator.SetBool("IsOpen", m_isOpened);
        return m_isOpened;
    }
}
