using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    public enum State
    {
        Move = 0,
        Attack,
        Hit
    }

    public State state = State.Move;
    public int hp = 100;

    NavMeshAgent agent;
    Animator an;

    public Camera cam;
    public LayerMask movementMask;


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

            if (Physics.Raycast(ray, out hit, 100, movementMask))
            {
                //Debug.Log(hit.point);
                agent.SetDestination(hit.point);
                agent.isStopped = false;
            }
        }

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.isStopped = true;
                an.SetFloat("Walk", 0.0f);
            }
        }

        an.SetFloat("Walk", agent.velocity.sqrMagnitude);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //state = state.attack;
            //an.settrigger("attack");
            if(state != State.Hit)
            {
                state = State.Hit;
                hp -= 10;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            state = State.Move;
        }
    }
}
