using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IFlash
{
    float stopT;
    bool isBool;
    NavMeshAgent nav;
    SkinnedMeshRenderer skinnedMeshRenderer;
    Color originColor;
    private void Awake()
    {
        if (GetComponent<NavMeshAgent>() == null)
        {
            nav = null;
            return;
        }
        nav = GetComponent<NavMeshAgent>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        originColor = skinnedMeshRenderer.material.color;
    }
    public void ApplyFlash(FlashMessage flashMessage)
    {
        if (flashMessage.isFlash) isBool = true;
    }

    void Update()
    {
        if(nav != null)
        {
            if (isBool)
            {
                nav.speed = 0;
                stopT += Time.deltaTime;
                skinnedMeshRenderer.material.color = Color.red;
                if (stopT > 5.0f)
                {
                    init();
                }
            }
            else
            {
                init();
            }
        }
    }

    void init()
    {
        nav.speed = 1.2f;
        stopT = 0f;
        isBool = false;
        skinnedMeshRenderer.material.color = originColor;
    }
}
