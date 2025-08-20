using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interaction
{
    [SerializeField] 
    private Animator m_animatorDoor;
    [SerializeField]
    private Collider m_interactionTriggerCol;

    public bool IsUnlocked { get; private set; } = false;
    public bool IsOpened { get; private set; } = false;

    public override void PlayInteraction()
    {

    }

    public void Open()
    {
        AudioManager.instance.PlayOpenedDoor();
        m_animatorDoor.SetBool("IsOpen", true);
    }

    public void Close()
    {
        m_animatorDoor.SetBool("IsClose", true);
    }

    public void Unlock()
    {
        IsUnlocked = true;
    }
}
