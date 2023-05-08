using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public LayerMask movementMask;
    Camera cam;

    PlayerMotor motor;
    Rigidbody rigid;
    private void Awake()
    {
        cam = Camera.main;

        motor = GetComponent<PlayerMotor>();
        rigid = GetComponent<Rigidbody>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, movementMask))
            {
                //Debug.Log(hit.point);
                motor.MoveToTarget(hit.point);
            }
        }

        rigid.velocity = Vector3.zero;
    }
}
