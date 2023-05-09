using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMotor : MonoBehaviour
{
    // NavMeshAgent를 통한 이동 또는 변화를 관리
    NavMeshAgent agent;
    Transform target;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        GetComponent<PlayerController>().onFocusChanged += OnFocusChanged;
    }

    private void OnDisable()
    {
        GetComponent<PlayerController>().onFocusChanged -= OnFocusChanged;
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

    void OnFocusChanged(Interactable newFocus)
    {
        if(newFocus!= null)
        {
            target = newFocus.guideTransform;
            agent.updateRotation = false;
            agent.stoppingDistance = newFocus.size;
        }
        else
        {
            target = null;
            agent.updateRotation = true;
            agent.stoppingDistance = 0.5f;
        }
    }
}
