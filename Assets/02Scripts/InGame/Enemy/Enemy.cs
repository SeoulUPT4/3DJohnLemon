using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Gargoyle,
    PatrolGhost,
    FollowGhost
}

public class Enemy : MonoBehaviour, IApplyFlash
{
    public EnemyType enemyType;
    private float m_stopTime;
    bool isLight;
    NavMeshAgent nav;
    SkinnedMeshRenderer skinnedMeshRenderer;
    Color originColor;
    private GameObject target;
    private bool isFollow;
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
    private void Start()
    {
        if(enemyType == EnemyType.PatrolGhost)
        {
            nav.enabled = false;
        }
    }

    public void ApplyFlash(bool isFlash)
    {
        isLight = isFlash;
        Debug.Log(this.gameObject.name);
    }

    void Update()
    {
        if(nav != null)
        {
            if (isLight)
            {
                nav.speed = 0;
                m_stopTime += Time.deltaTime;
                skinnedMeshRenderer.material.color = Color.red;
                if (m_stopTime > 5.0f)
                {
                    init();
                }
            }
            else if(!isLight)
            {
                if (enemyType == EnemyType.PatrolGhost)
                {
                    target = PlayerController.Instance.gameObject;
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * 0.8f);
                    Vector3 dir = target.transform.position - transform.position;
                    Vector3 newDir = Vector3.RotateTowards(transform.forward, dir, Time.deltaTime * 2.0f, 0f);
                    transform.rotation = Quaternion.LookRotation(newDir);
                }
            }
            else
            {
                init();
            }
        }

        if (enemyType == EnemyType.FollowGhost)
        {
            target = PlayerController.Instance.gameObject;
            nav.SetDestination(target.transform.position);
        }
    }

    void init()
    {
        if (enemyType == EnemyType.FollowGhost) nav.speed = 0.6f;
        else nav.speed = 1.2f;
        m_stopTime = 0f;
        isLight = false;
        skinnedMeshRenderer.material.color = originColor;
    }
}
