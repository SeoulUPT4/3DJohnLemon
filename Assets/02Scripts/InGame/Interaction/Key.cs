using System.Collections.Generic;
using UnityEngine;

public class Key : InteractionComponent
{
    [SerializeField] 
    private GameObject m_doorLine;

    private void Start()
    {
        if(m_doorLine != null)
            m_doorLine.SetActive(false);
    }
    public override bool Interact(PlayerInventory inventory)
    {
        if (inventory == null || inventory.HasKey(InteractionData.ID)) return false;

        PickUpKey();
        inventory.AddKey(InteractionData.ID);
        return true;
    }
    private void PickUpKey()
    {
        AudioManager.Instance.PlayPickUpSound();
        if (m_doorLine != null)
            m_doorLine.SetActive(true);
        Destroy(this.gameObject);
        //this.gameObject.SetActive(false);
    }
}
