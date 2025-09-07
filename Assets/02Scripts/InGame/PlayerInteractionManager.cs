using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerInventory))]
public class PlayerInteractionManager : MonoBehaviour
{
    [SerializeField]
    private PlayerInput m_input;
    private PlayerInventory m_inventory;
    private List<InteractionComponent> m_nearInteractionList = new();

    private void Awake()
    {
        m_input = GetComponent<PlayerInput>();
        m_inventory = GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        if (m_input.IsInteraction) // F키
        {
            Debug.Log("Interaction");
            foreach (var interaction in m_nearInteractionList)
            {
                if (interaction.Interact(m_inventory))
                {
                    UIManager.Instance.SetInteractionUI(false);
                    break;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detected InteractionUI
        if (other.gameObject.layer == LayerMask.NameToLayer("Interaction"))
        {
            GameObject partent = other.transform.parent.gameObject;
            if (partent.TryGetComponent(out InteractionComponent interaction))
            {
                m_nearInteractionList.Add(interaction);
            }
            UIManager.Instance.SetInteractionUI(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        /*if (other.gameObject.layer == LayerMask.NameToLayer("Interaction"))
        {
            UIManager.Instance.SetInteractionUI(true);
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interaction"))
        {
            GameObject partent = other.transform.parent.gameObject;
            if (partent.TryGetComponent(out InteractionComponent interaction))
            {
                UIManager.Instance.SetInteractionUI(false);
                m_nearInteractionList.Remove(interaction);
            }
            
        }
    }
}
