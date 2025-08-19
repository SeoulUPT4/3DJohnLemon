using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObject : MonoBehaviour
{
    
    public enum ID
    {
        Scene1_Exit = 0,
        Scene1_Door1 = 1,
    }

    public ID id;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            UIManager.Instance.SetInteractionUI(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.SetInteractionUI(false);
        }
    }
}
