using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputContorller : MonoBehaviour
{
    /// <summary>
    /// 마우스 클릭했을 때 실행되는 델리게이트
    /// </summary>
    public Action<Vector2> onClick;

    /// <summary>
    /// 마우스가 움직일 때 실행될 델리게이트
    /// </summary>
    public Action<Vector2> onMouseMove;

    /// <summary>
    /// 마우스 휠버튼을 돌릴 때 실행될 델리게이트
    /// </summary>
    public Action<float> onMouseWheel;

    PlayerInputAction inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Click.performed += OnClick;
        inputActions.Player.MousMove.performed += OnMouseMove;
        inputActions.Player.MouseWheel.performed += OnMouseWheel;
    }

    private void OnDisable()
    {
        inputActions.Player.MouseWheel.performed -= OnMouseWheel;
        inputActions.Player.MousMove.performed -= OnMouseMove;
        inputActions.Player.Click.performed -= OnClick;
        inputActions.Player.Enable();
    }

    private void OnClick(InputAction.CallbackContext _)
    {
        // 클릭 되었을 때 마우스 위치를 델리게이트로 전달
        onClick?.Invoke(Mouse.current.position.ReadValue());
    }
    private void OnMouseMove(InputAction.CallbackContext context)
    {
        // 마우스가 움직일 때 마우스의 위치를 델리게이트로 전달
        onMouseMove?.Invoke(context.ReadValue<Vector2>());
    }
    private void OnMouseWheel(InputAction.CallbackContext context)
    {
        // 마우스 휠버튼이 돌아갈 때 돌아가는 방향과 정도를 델리게이트로 전달
        // 위로 돌릴 때 +120. 아래로 돌릴 때 -120
        onMouseWheel?.Invoke(context.ReadValue<float>());
    }
}
