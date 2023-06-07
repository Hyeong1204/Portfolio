using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MouseFollow : MonoBehaviour
{
    [Range(1.0f, 20.0f)]
    public float distance = 10.0f;
    PlayerInputAction inputAction;

    private void Awake()
    {
        inputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputAction.Effect.Enable();
        inputAction.Effect.CousorMove.performed += OnMoueMove;
    }

    private void OnDisable()
    {
        inputAction.Effect.CousorMove.performed -= OnMoueMove;
        inputAction.Effect.Disable();
    }

    private void OnMoueMove(InputAction.CallbackContext context)
    {
        Vector3 mousePos = context.ReadValue<Vector2>();
        mousePos.z = 10.0f;
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = target;
    }
}
