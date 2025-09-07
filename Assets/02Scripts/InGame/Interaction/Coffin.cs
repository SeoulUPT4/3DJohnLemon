using UnityEngine;

public class Coffin : InteractionComponent
{
    public Animator animator;
    public bool m_isOpened = false;
    public override bool Interact(PlayerInventory inventory)
    {
        if (m_isOpened) return false;
        m_isOpened = !m_isOpened;
        animator.SetBool("IsOpen", m_isOpened);
        return m_isOpened;
    }
}
