using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMotor : MonoBehaviour
{
    // NavMeshAgent를 통한 이동 또는 변화를 관리
    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //if (!agent.pathPending)
        //{
        //    if (agent.remainingDistance <= agent.stoppingDistance)
        //    {
        //    }
        //}
    }

    public void MoveToTarget(Vector3 position)
    {
        agent.SetDestination(position);
    }
}
