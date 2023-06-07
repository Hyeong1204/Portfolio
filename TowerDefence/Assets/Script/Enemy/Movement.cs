using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    float originalMoveSpeed = 0.0f;

    Vector3 moveDir = Vector3.zero;

    bool isStun = false;

    SpriteRenderer rend;

    public float MoveSpeed
    {
        get => moveSpeed;
        set
        {
            moveSpeed = value;
        }
    }

    /// <summary>
    /// 스턴 확인상태 프러퍼티
    /// </summary>
    public bool IsStun
    {
        set
        {
            isStun = value;
        }
    }

    public float OriginalMoveSpeed => originalMoveSpeed;

    private void Start()
    {
        originalMoveSpeed = MoveSpeed;
        rend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!isStun)            // 스턴 상태가 아니면
        {
            transform.position += moveSpeed * Time.deltaTime * moveDir; 
            //transform.rotation = Quaternion.LookRotation(moveDir);
            if(moveDir.x > 0)
            {
                rend.flipX = true;
            }
            else
            {
                rend.flipX = false;
            }
        }
    }

    public void MoveTo(Vector3 newPosition)
    {
        moveDir = newPosition;      // newPosition으로 방향 설정
    }
}
