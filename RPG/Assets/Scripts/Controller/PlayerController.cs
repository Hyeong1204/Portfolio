using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public LayerMask movementMask;
    public LayerMask interactionMask;

    public Interactable focus;

    Camera cam;

    PlayerMotor motor;
    Rigidbody rigid;

    public delegate void OnFocusChanged(Interactable newFocus);
    public OnFocusChanged onFocusChanged;

    private void Awake()
    {
        cam = Camera.main;

        motor = GetComponent<PlayerMotor>();
        rigid = GetComponent<Rigidbody>();
    }


    void Update()
    {
        rigid.velocity = Vector3.zero;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, movementMask))
            {
                //Debug.Log(hit.point);
                motor.MoveToTarget(hit.point);
                DeFocus();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, interactionMask))
            {
                SetFocus(hit.collider.GetComponent<Interactable>());
            }
        }
    }

    void SetFocus(Interactable newFocus)
    {
        onFocusChanged?.Invoke(newFocus);

        //if (focus != newFocus && newFocus != null)
        //{
        //    focus = newFocus;
        //    motor.MoveToTarget(focus.guideTransform.position);
        //}
    }

    void DeFocus()
    {
        focus = null;
    }
}
