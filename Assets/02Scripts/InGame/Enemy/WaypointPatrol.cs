using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointPatrol : MonoBehaviour
{
    public Transform[] waypoints;

    int m_CurrentWaypoointIndex;

    /*void Update()
    {
        if (navMeshAgent.enabled == false) return;
        else if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            m_CurrentWaypoointIndex = (m_CurrentWaypoointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypoointIndex].position);
        }
    }*/

    public void Partrol()
    {

    }
}
