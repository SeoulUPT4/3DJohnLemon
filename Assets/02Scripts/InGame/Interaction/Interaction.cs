using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType
{
    Key,
    Door
}
public abstract class Interaction : MonoBehaviour, IInteractiveBehaviour
{
    public InteractionType InteractionType;

    public abstract void PlayInteraction();
}
