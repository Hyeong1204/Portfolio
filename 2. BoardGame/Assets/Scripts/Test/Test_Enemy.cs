using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Test_Enemy : TestBase
{
    public Button reset;
    public Button randomDeplyment;
    public Button resetRandom;
    public PlayerBase player1;      // 나
    public PlayerBase player2;      // 적


    Board board1;
    Board board2;

    protected override void Awake()
    {
        base.Awake();
        GameObject canvas = GameObject.Find("Canvas");
        reset = canvas.transform.GetChild(0).GetComponent<Button>();
        randomDeplyment = canvas.transform.GetChild(1).GetComponent<Button>();
        resetRandom = canvas.transform.GetChild(2).GetComponent<Button>();
    }

    private void Start()
    {
        board1 = player1.GetComponentInChildren<Board>();
        board2 = player2.GetComponentInChildren<Board>();

        reset.onClick.AddListener(OnResetClick);
        resetRandom.onClick.AddListener(() =>
        {
            OnResetClick();
            player1.AutoShipDeployment(false);
            player2.AutoShipDeployment(false);
        });

        randomDeplyment.onClick.AddListener(() =>
        {
            player1.AutoShipDeployment(false);
            player2.AutoShipDeployment(false);
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
        // 진행 중이던 게임 리셋
        player1.Clear();
        player2.Clear();
    }

    private void OnClick(InputAction.CallbackContext _)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);        
        Vector2Int giridPos = board2.WorldToGrid(worldPos);
        player1.Attack(giridPos);
    }

    private void OnRClick(InputAction.CallbackContext context)
    {
        // 마우스 위치에 있는 함선을 배치 취소
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        ShipType type = board1.GetShipType(worldPos);
        if (type != ShipType.None)
        {
            Ship ship = player1.Ships[(int)type - 1];
            board1.UndoShipDeplyment(ship);
        }
    }

    private void OnTestWheel(InputAction.CallbackContext context)
    {


    }

    private void OnTestMove(InputAction.CallbackContext context)
    {

    }
}
