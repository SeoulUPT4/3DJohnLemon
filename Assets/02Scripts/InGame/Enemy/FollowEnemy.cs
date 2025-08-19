using UnityEngine;

public class FollowEnemy : Enemy
{
    private bool isFollow = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        m_CurrentState = EnemyState.Chase;
    }

    public void StartFollow()
    {
        isFollow = true;
    }
    protected override void BaseState()
    {
        m_CurrentState = EnemyState.Chase;
    }
    protected override void ChaseTarget()
    {
        if (!isFollow) return;
        Vector3 _dir = (m_Target.transform.position - this.transform.position).normalized;

        Debug.DrawLine(transform.position, transform.position + _dir * 50);
        transform.Translate(_dir * Time.deltaTime, Space.World);
        transform.LookAt(m_Target.transform.position);
    }
}
