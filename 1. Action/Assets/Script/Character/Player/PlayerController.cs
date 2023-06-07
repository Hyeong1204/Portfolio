using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInputAction playerInput;
    Animator anima;
    CharacterController cc;
    Player player;

    public float rotateSpeed = 180.0f;              // 회전속도
    public float walkSpeed = 3.0f;                 // 걷는 이동속도
    public float runSpeed = 5.0f;                  // 달리는 이동속도

    float currentSpeed = 3.0f;                      // 현재 이동속도

    /// <summary>
    /// 이동 상태를 나타내는 enum
    /// </summary>
    enum MoveMode
    {
        Walk = 0,
        Run
    }

    /// <summary>
    /// 현재 이동 상태
    /// </summary>
    MoveMode moveMode = MoveMode.Walk;

    Vector3 moveDir = Vector3.zero;                 // 입력으로 지정된 바로보는 방향
    Quaternion targerRotation = Quaternion.identity;        // 최종 회전 목표

    private void Start()
    {
        InventoryUI invenui = Gamemanager.Inst.InvenUI;
        invenui.onInevntoryOpen += () => playerInput.Player.Disable();
        invenui.onInevntoryClose += () => playerInput.Player.Enable();
    }

    private void Awake()
    {
        playerInput = new PlayerInputAction();
        anima = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        playerInput.Player.Enable();            // 인풋 액션에서 액션맵 활성화
        playerInput.Player.Move.performed += OnMove;        // 액션과 함수 연결
        playerInput.Player.Move.canceled += OnMove;
        playerInput.Player.Dash.performed += OnDash;        // 액션과 대쉬 함수 연결
        playerInput.Player.Attack.performed += OnAttack;
        playerInput.Player.Pickup.performed += OnPickup;
        playerInput.Player.LockOn.performed += OnLockOn;
        //playerInput.Player.Dash.canceled += OnDash;
    }


    private void OnDisable()
    {
        //playerInput.Player.Dash.canceled -= OnDash;
        playerInput.Player.LockOn.performed -= OnLockOn;
        playerInput.Player.Pickup.performed -= OnPickup;
        playerInput.Player.Attack.performed -= OnAttack;
        playerInput.Player.Dash.performed -= OnDash;        // 액션과 대쉬 함수 해재
        playerInput.Player.Move.canceled -= OnMove;         // 역션과 함수 연결 해제
        playerInput.Player.Move.performed -= OnMove;
        playerInput.Player.Disable();           // 액션맵 비활성화
    }

    private void Update()
    {
        if (player.IsAlive)         //살아있을 때만 업데이트
        {
            //cc.Move(currntSpeed * Time.deltaTime * moveDir);
            cc.Move(currentSpeed * Time.deltaTime * moveDir);

            // 락온한 대상이 잇으면 락온한 대상의 위치를 바라보게 만들기
            if(player.LockOnTransform != null)
            {
                targerRotation = Quaternion.LookRotation(player.LockOnTransform.position - player.transform.position);
            }

            Rotate();
        }
    }

    /// <summary>
    /// PlayerAction맵의 Move 액션이 perfromed, canceled될 때 실행
    /// </summary>
    /// <param name="context"></param>
    private void OnMove(InputAction.CallbackContext context)
    {
        // WASD 입력을 받아옴(+x :D, -x : A, +y : W, -y : S) 
        Vector2 dir = context.ReadValue<Vector2>();
        moveDir.z = dir.y;          // 입력받은 것을 3D xz평면상의 방향으로 변경
        moveDir.y = 0.0f;
        moveDir.x = dir.x;

        if (!context.canceled && player.IsAlive)      // 살아있고 입력이 들어왔을 때만 실행되는 코드
        {
            Quaternion cameraYRotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);  // 카메라의 y축 회전만 분리
            moveDir = cameraYRotation * moveDir;        // 카메라의 y축 회전을 moveDir에 곱한다. => moveDir과 카메라가 xz평면상에서 바라보는 방향을 일치시킴
            targerRotation = Quaternion.LookRotation(moveDir);      // moveDir방향으로 바라보는 회전 만들기
            

            if(moveMode == MoveMode.Walk)
            {
                anima.SetFloat("Speed", 0.3f);      // Walk모드면 걷는 애니메이션
            }
            else
            {
                anima.SetFloat("Speed", 1.0f);      // Run모드면 달리는 애니메이션
            }
        }
        else
        {
            moveDir = Vector3.zero;
            anima.SetFloat("Speed", 0.0f);          // 입력이 안들어 왔으면 대기 애니메이션
        }
    }

    /// <summary>
    /// 아이템 획득 버튼을 눌렀을 때 실행
    /// </summary>
    /// <param name="_"></param>
    private void OnPickup(InputAction.CallbackContext _)
    {
        player.ItemPickup();
    }

    /// <summary>
    /// 쉬프트 키를 눌렀을 때 실행
    /// </summary>
    /// <param name="_"></param>
    private void OnDash(InputAction.CallbackContext _)
    {
        if(moveMode == MoveMode.Walk)
        {
            // Walk모드면 Run모드로 전환
            moveMode = MoveMode.Run;
            currentSpeed = runSpeed;     // 이동 속도를 달리는 속도로 변경
            if (moveDir != Vector3.zero)
            {
                anima.SetFloat("Speed", 1.0f);      // 움직이는 중일때만 Run모드면 달리는 애니메이션
            }
        }
        else
        {
            // Run모드면 Walk모드로 전환
            moveMode = MoveMode.Walk;
            currentSpeed = walkSpeed;    // 이동 속도를 걷는 속도로 변경
            if (moveDir != Vector3.zero)
            {
                anima.SetFloat("Speed", 0.3f);      // 움직이는 중일때만 Walk모드면 걷는 애니메이션
            }
        }
    }

    void OnMove()
    {
        transform.Translate(Time.deltaTime * currentSpeed * moveDir, Space.World);     // 초당 moveSpeed의 속도로 movDir방향으로 이동 (월드 스페이스 기준)
    }

    void Rotate()
    {
        // transform.rotation에서 targetRotation으로 초당 1/rotateSpeed씩 보간
        transform.rotation = Quaternion.Slerp(transform.rotation, targerRotation, Time.deltaTime * rotateSpeed);
    }

    /// <summary>
    /// 스페이스나 키나 마우스 왼클릭 했을 때 실행
    /// </summary>
    /// <param name="_"></param>
    private void OnAttack(InputAction.CallbackContext _)
    {
        // anima.GetCurrentAnimatorStateInfo(0).normalizedTime; 현재 재생중인 애니메이션의 진행 상태를 알려줌 (0~1)
        if(player.IsAlive)       // 살아있으면 공격
        {
            int comboState = anima.GetInteger("ComboState");        // ComboState를 애니메이터에서 읽어와서
            comboState++;       // 콤보 상태 1증가 시키기
            anima.SetInteger("ComboState", comboState);             // 애니메이터에 증가된 콤보 상태 설정
            anima.SetTrigger("Attack");                             // Attack 트리거 발동
        }
    }

    /// <summary>
    /// 몬스터 락온 버튼을 눌럿을 때 실행
    /// </summary>
    /// <param name="_"></param>
    private void OnLockOn(InputAction.CallbackContext _)
    {
        player.LockOnToggle();
    }
}
