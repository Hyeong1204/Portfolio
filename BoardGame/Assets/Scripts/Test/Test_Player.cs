using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Test_Player: TestBase
{
    public Button reset;
    public Button randomDeplyment;
    public Button resetRandom;
    public PlayerBase player;


    Board board;


    private void Start()
    {
        board = FindObjectOfType<Board>();
        
        reset.onClick.AddListener(OnResetClick);
        resetRandom.onClick.AddListener(() =>
        {
            OnResetClick();
            player.AutoShipDeployment(false);
        });

        randomDeplyment.onClick.AddListener(() =>
        {
            player.AutoShipDeployment(false);
        });
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        inputActions.Test.TestClick.performed += OnClick;
        inputActions.Test.Test_RClick.performed += OnRClick;
        inputActions.Test.TestWneel.performed += OnTestWheel;
        inputActions.Test.Test_MouseMove.performed += OnTestMove;
    }

    protected override void OnDisable()
    {
        inputActions.Test.Test_MouseMove.performed -= OnTestMove;
        inputActions.Test.TestWneel.performed -= OnTestWheel;
        inputActions.Test.Test_RClick.performed -= OnRClick;
        inputActions.Test.TestClick.performed -= OnClick;
        base.OnEnable();
    }

    private void OnResetClick()
    {
        // 아직 배치 되지 않은 함선을 배치
        player.UndoAllShipDeployment();
    }

    private void OnClick(InputAction.CallbackContext _)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        
        Vector2Int giridPos = board.WorldToGrid(worldPos);
        board.OnAttacked(giridPos);
    }

    private void OnRClick(InputAction.CallbackContext context)
    {
        // 마우스 위치에 있는 함선을 배치 취소
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        ShipType type = board.GetShipType(worldPos);
        if (type != ShipType.None)
        {
            Ship ship = player.Ships[(int)type - 1];
            board.UndoShipDeplyment(ship);
        }
    }

    private void OnTestWheel(InputAction.CallbackContext context)
    {


    }

    private void OnTestMove(InputAction.CallbackContext context)
    {

    }
}
