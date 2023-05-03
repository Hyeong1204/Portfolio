using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    public Camera cam;
    NavMeshAgent agent;
    public LayerMask movementMask;

    Animator an;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        an = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100, movementMask))
            {
                //Debug.Log(hit.point);
                agent.SetDestination(hit.point);
                agent.isStopped = false;
                an.SetInteger("Walk", 1);
            }
        }

        if (!agent.pathPending)
        {
            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.isStopped = true;
                an.SetInteger("Walk", 0);
            }
        }
    }
}
