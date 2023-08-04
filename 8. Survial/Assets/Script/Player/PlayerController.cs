using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;            // 스피드 조정 변수

    [SerializeField]
    private float runSpeed;
    private float applySpeed;

    [SerializeField]
    private float crouchSpeed;

    [SerializeField]
    private float jumpForce;

    // 상태 변수
    private bool isWalk = false;
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;

    // 움직임 체크 변수
    private Vector3 lastPos;

    // 앉았을 때 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    [SerializeField]
    private float lookSensitivity;      // 카메라 민감도

    [SerializeField]
    private float cameraRotationLimit;      // 카메라 각 최대치 (위 아래)
    private float currentCameraRotateionX = 0;

    private Rigidbody myRigid;
    private Camera theCamera;
    private CapsuleCollider capsuleCollider;        // 땅 착지 여부
    private GunControlloer theGunController;
    private Crosshair theCrosshair;

    private void Awake()
    {
        myRigid = GetComponent<Rigidbody>();
        theCamera = Camera.main;
        capsuleCollider = GetComponent<CapsuleCollider>();
        applySpeed = walkSpeed;
    }

    private void Start()
    {
        theGunController = FindObjectOfType<GunControlloer>();
        theCrosshair = FindObjectOfType<Crosshair>();
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
        lastPos = transform.position;
    }

    private void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        MoveCheck();
        CameraRotation();
        CharacterRotation();
    }

    /// <summary>
    /// 앉기 시도
    /// </summary>
    private void TryCrouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    /// <summary>
    /// 앉기 동작
    /// </summary>
    private void Crouch()
    {
        isCrouch = !isCrouch;
        theCrosshair.CrouchAnimation(isCrouch);

        if (isCrouch )
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
    }

    /// <summary>
    /// 부드러운 앉기 동작 실행
    /// </summary>
    /// <returns></returns>
    IEnumerator CrouchCoroutine()
    {
        float posY = theCamera.transform.localPosition.y;
        int count = 0;

        while (posY != applyCrouchPosY)
        {
            count++;
            posY = Mathf.Lerp(posY, applyCrouchPosY, Time.deltaTime * 10);
            theCamera.transform.localPosition = new Vector3(0, posY, 0);

            if (count > 15) break;

            yield return null;
        }

        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }

    /// <summary>
    /// 지면 체크
    /// </summary>
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);      // 캡슐콜라이더의 y축 방향으로 반 + 0.1 하여 땅의 닿았는지 탐색
        theCrosshair.RunninggAnimation(!isGround);
    }

    /// <summary>
    /// 점프 시도
    /// </summary>
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }    

    /// <summary>
    /// 달리기 시도
    /// </summary>
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCanel();
        }
    }

    /// <summary>
    /// 달리기 실행
    /// </summary>
    private void Running()
    {
        if (isCrouch) Crouch();      // 앉은 상태에서 달리기시 앉은 상태 해제

        theGunController.CancelFineSight();     // 뛰면 정조준 모드 풀기

        isRun = true;
        theCrosshair.RunninggAnimation(isRun);
        applySpeed = runSpeed;
    }

    /// <summary>
    /// 달리기 취소
    /// </summary>
    private void RunningCanel()
    {
        isRun = false;
        theCrosshair.RunninggAnimation(isRun);
        applySpeed = walkSpeed;
    }

    /// <summary>
    /// 점프
    /// </summary>
    private void Jump()
    {
        if(isCrouch) Crouch();      // 앉은 상태에서 점프시 앉은 상태 해제

        myRigid.velocity = transform.up * jumpForce;
    }

    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirz = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirz;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
    }

    private void MoveCheck()
    {
        if (!isRun && !isCrouch && isGround)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                isWalk = true;
            }
            else
            {
                isWalk = false;
            }

            theCrosshair.WalkingAnimation(isWalk);
            lastPos = transform.position; 
        }
    }

    /// <summary>
    /// 좌우 캐릭터 회전
    /// </summary>
    private void CharacterRotation()
    {        
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationY = new Vector3(0.0f, yRotation, 0.0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(characterRotationY));
    }

    /// <summary>
    /// 상하 카메라 회전
    /// </summary>
    private void CameraRotation()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotaionX = xRotation * lookSensitivity;

        currentCameraRotateionX -= cameraRotaionX;
        currentCameraRotateionX = Mathf.Clamp(currentCameraRotateionX, -cameraRotationLimit, cameraRotationLimit);  // cameraRotationLimit 최대각 만큼 막기

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotateionX, 0.0f, 0.0f);
    }

}
