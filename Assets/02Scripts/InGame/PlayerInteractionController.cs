using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField]
    private PlayerInput m_input;
    private List<Interaction> m_keyList = new List<Interaction>();
    public List<Interaction> nearkeyList = new List<Interaction>();

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip sound_pick;

    public Door nearDoor;

    private void Awake()
    {
        m_input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        /*// Key 인터랙션
        if (nearkeyList.Count > 0)
        {
            InteractionUI_key.enabled = true;

            if (Input.GetKeyDown(KeyCode.F))
            {
                KeyObject key = nearkeyList[nearkeyList.Count - 1];
                nearkeyList.RemoveAt(nearkeyList.Count - 1);
                PickUpKey(key);
            }
        } else
        {
            InteractionUI_key.enabled = false;
        }

        // Door 인터랙션
        if (nearDoor)
        {
            InteractionUI_door.enabled = true;

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (nearDoor.IsOpened)
                {
                    CloseDoor(nearDoor);
                } else
                {
                    OpenDoor(nearDoor);
                }
            }
        } else
        {
            InteractionUI_door.enabled = false;
        }*/
    }

    public void OpenDoor(Door door)
    {
        /*if (door.IsUnlocked) 
        {
            door.Open();
            return;
        }

        *//*KeyObject key = keyList.Find(key => key.id == door.keyID);
        if (key != null)
        {
            door.Unlock();
        }*//*

        door.Open();*/
    }

    public void CloseDoor(Door door)
    {
        //door.Close();
    }

    public void PickUpKey(Interaction key)
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        // Interaction
        if (other.gameObject.layer == LayerMask.NameToLayer("Interaction"))
        {
            UIManager.Instance.SetInteractionUI(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interaction"))
        {
            if(m_input.IsInteraction)
            {
                IInteractiveBehaviour _interactiveBehaviour = other.GetComponent<IInteractiveBehaviour>();
                _interactiveBehaviour.PlayInteraction();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interaction"))
        {
            UIManager.Instance.SetInteractionUI(false);
        }
    }
}
