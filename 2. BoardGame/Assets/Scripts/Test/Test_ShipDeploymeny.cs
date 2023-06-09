using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ShipDeploymeny : TestBase
{
    Board board;

    Ship targetShip = null;
    Ship TargetShip
    {
        get => targetShip;
        set
        {
            if (targetShip != value)
            {
                if (targetShip != null)                         // 이전 targetShip에 대한 처리

                {
                    targetShip.SetMaterialType();               // 함선의 머티리얼 타입을 normal로 돌리기
                    targetShip.gameObject.SetActive(false);     // 비활성화 시키기
                }

                targetShip = value;         // targetShip 변경

                if (targetShip != null)                         // 새 targetShip이 있으면 그것에 대한 처리
                {
                    targetShip.SetMaterialType(false);          // 함선의 머티리얼 타입을 deployMode로 변경

                    Vector2Int girdPos = board.GetMouseGridPosition();              // 마우스 커서의 그리드 좌표 계산해서
                    TargetShip.transform.position = board.GridToWorld(girdPos);     // 해당위치에 함선 배치

                    targetShip.gameObject.SetActive(true);                          // 활성화 시켜서 보이게 만들기
                }
            }
        }
    }

    Ship[] testShips = null;

    private void Start()
    {
        board = FindObjectOfType<Board>();
        testShips = new Ship[ShipManager.Inst.ShipTpyeCount];
        testShips[(int)ShipType.Carrier - 1] = ShipManager.Inst.MakeShip(ShipType.Carrier, this.transform);
        testShips[(int)ShipType.Battleship - 1] = ShipManager.Inst.MakeShip(ShipType.Battleship, this.transform);
        testShips[(int)ShipType.Destroyer - 1] = ShipManager.Inst.MakeShip(ShipType.Destroyer, this.transform);
        testShips[(int)ShipType.Submarine - 1] = ShipManager.Inst.MakeShip(ShipType.Submarine, this.transform);
        testShips[(int)ShipType.PatrolBoat - 1] = ShipManager.Inst.MakeShip(ShipType.PatrolBoat, this.transform);
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



    private void OnClick(InputAction.CallbackContext _)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);

        if (TargetShip != null && board.ShipDeplyment(TargetShip, world))       // TargetShip 있으면 함선 배치 시도
        {
            TargetShip = null;          // TargetShip 있고 함선 배치가 성공했으면 TargetShip 해제
        }
    }

    private void OnRClick(InputAction.CallbackContext _)
    {
        if (TargetShip != null)
        {
            board.UndoShipDeplyment(TargetShip);
            TargetShip = null;
        }
    }

    private void OnTestWheel(InputAction.CallbackContext context)
    {
        float delta = context.ReadValue<float>();

        bool ccw = false;       // 기본적으로 시계방향

        if (delta > 0.0f)       // 휠을 올리면
        {
            ccw = true;         // 반시계 방향으로
        }

        if (TargetShip != null) // TagetShip이 있으면
        {
            TargetShip.Rotate(ccw);     // 횔방향에 따라 회전
            
            bool isSuccess = board.IsShipDeployment(targetShip, targetShip.transform.position); // 배치 가능한지 확인
            ShipManager.Inst.SetDeployModeColor(isSuccess);     // 머티리얼 업데이트
        }

    }

    private void OnTestMove(InputAction.CallbackContext context)
    {
        if (TargetShip != null && !targetShip.IsDeployed)
        {
            Vector2Int girdPos = board.GetMouseGridPosition();              // 마우스 커서 위치를 그리드로 계산
            TargetShip.transform.position = board.GridToWorld(girdPos);     // 계산한 위치로 TargetShip 옮기기

            bool isSuccess = board.IsShipDeployment(targetShip, girdPos);   // 배치 가능한지 확인해서 머티리얼 업데이트
            ShipManager.Inst.SetDeployModeColor(isSuccess);
        }
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        TargetShip = testShips[(int)ShipType.Carrier - 1];
    }
    protected override void Test2(InputAction.CallbackContext _)
    {
        TargetShip = testShips[(int)ShipType.Battleship - 1];
    }
    protected override void Test3(InputAction.CallbackContext _)
    {
        TargetShip = testShips[(int)ShipType.Destroyer - 1];
    }
    protected override void Test4(InputAction.CallbackContext _)
    {
        TargetShip = testShips[(int)ShipType.Submarine - 1];
    }
    protected override void Test5(InputAction.CallbackContext _)
    {
        TargetShip = testShips[(int)ShipType.PatrolBoat - 1];
    }
}
