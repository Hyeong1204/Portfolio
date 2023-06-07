using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Board : TestBase
{
    Board board;

    private void Start()
    {
        board = FindObjectOfType<Board>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        inputActions.Test.TestClick.performed += OnClick;
    }

    protected override void OnDisable()
    {
        inputActions.Test.TestClick.performed -= OnClick;
        base.OnEnable();
    }

    private void OnClick(InputAction.CallbackContext _)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);
        Vector2Int grid = board.WorldToGrid(world);
        Debug.Log($"클릭 그리드 : {grid.x}, {grid.y}");
        Vector3 GtoW = board.GridToWorld(grid);
        Debug.Log($"클릭 그리드 : {GtoW.x}, {GtoW.z}");
    }

    protected override void Test1(InputAction.CallbackContext _)
    {

    }
    
}
