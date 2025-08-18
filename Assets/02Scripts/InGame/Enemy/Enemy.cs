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

public enum EnemyState
{
    Idle,
    Patrol,
    Follow,
}

public class Enemy : MonoBehaviour, IApplyFlash
{
    [SerializeField] private EnemyType m_enemyType;
    [SerializeField] private GameObject m_target;
    [Space(10)]

    [Header("[ AI ]")]
    [SerializeField] private float m_chaseDistance;
    [SerializeField] private float m_stopDistance;  // 추적 최소거리
    [Space(10)]

    [Header("[ FlashLight Hit ]")]
    [SerializeField] ParticleSystem m_particles;
    [SerializeField] private float m_flashStopTime; 
    

    private NavMeshAgent m_navAgent;
    private SkinnedMeshRenderer m_skinnedMesh;
    private Color m_originColor;
    private EnemyState m_enemyState = EnemyState.Idle;
    private bool m_isHitFlash;

    private void Awake()
    {
        if (GetComponent<NavMeshAgent>() != null)
            m_navAgent = GetComponent<NavMeshAgent>();

        m_skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        m_particles = GetComponentInChildren<ParticleSystem>();
        m_originColor = m_skinnedMesh.material.color;
    }
    public void init()
    {
        /*if (enemyType == EnemyType.FollowGhost) nav.speed = 0.6f;
        else nav.speed = 1.2f;
        m_stopTime = 0f;
        isLight = false;
        skinnedMeshRenderer.material.color = originColor;*/
    }

    private void Start()
    {
        m_particles.Stop();
        /*if(enemyType == EnemyType.PatrolGhost)
        {
            nav.enabled = false;
        }*/
    }


    void Update()
    {
        float _distance = Vector3.Distance(m_target.transform.position, transform.position);

        if (m_isHitFlash) return;
        // 추적 거리에 들어왔을 시
        if(_distance <= m_chaseDistance)
        {
            ChaseTarget();
        }
        else
        {
            BaseState();
        }
        /*if(nav != null)
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
        }*/
    }

    public void ApplyFlash(float flashDuration)
    {
        if (m_isHitFlash) return;
        m_isHitFlash = true;
        HitFlash(flashDuration);
    }
    private void ChaseTarget()
    {

    }

    protected virtual void BaseState()
    {

    }

    private void HitFlash(float flashDuration)
    {
        Debug.Log("Hit");
        Color _color = new Color();
        _color = Color.red;

        m_skinnedMesh.material.color = _color;
        m_particles.Play();
        StartCoroutine(StopEnemyCoroutine(flashDuration));
    }

    private IEnumerator StopEnemyCoroutine(float flashDuration)
    {
        yield return new WaitForSeconds(m_flashStopTime + m_flashStopTime);
        m_particles.Stop();

        m_skinnedMesh.material.color = m_originColor;
    }


}
