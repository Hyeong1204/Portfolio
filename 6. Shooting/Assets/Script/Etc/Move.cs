using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Move : MonoBehaviour
{
    public float speed = 1.0f;

    Vector3 dir;
    // 유니티 이벤트 함수 : 유니티가 특정 타이밍에 실행 시키는 함수

    /// <summary>
    /// Start 함수. 게임이 시작될 때(첫번째 update 함수가 호출되는 직전에 호출되는 함수) 호출되는 함수
    /// </summary>
    private void Start()
    {
        Debug.Log("Hollo unity");
    }
     

    /// <summary>
    /// Update 함수. 매 프레임마다 호쵤되는 함수. 지속으로 변경되는 것이 있을 때 사용하는 함수.
    /// </summary>
    private void Update()
    {
    //    // Vector3 : 백터를 표현하기 위한 구조체. 위치를 표현할 때도 많이 사용한다.
    //    // 백터 : 힘의 방향과 크기를 나타내는 단위

    //    //transform.position += (Vector3.left * speed);   // 아래쪽 방향으로 speed 만큼 움직여라(매 프레임마다)
    //    //transform.position += new Vector3(1, 0, 0); 
    //    //new Vector3(0, 1, 0);   // 위쪽
    //    //new Vector3(0, -1, 0);   // 아래쪽

    //    //  Time.deltaTime : 이전 프레임에서 지금 프레임까지 걸린 시간
    //    //transform.position += (speed * Time.deltaTime * Vector3.up);  // 아래쪽 방향으로 speed 만큼 움직여라(매 초마다)

    //    //Test_OldInputManager();
        
        transform.position += (speed * Time.deltaTime * dir);

    //    // Big - Little 
    //    // sleep

    //    // input system
    //    // Enent-driven(이벤트 드리븐) 방식으로 구현 -> 일이 있을 때만 동작한다. (전력을 아끼기에 적합한 구조)
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
       
            Debug.Log("입력들어옴 - 2");
            Vector2 inputdir = context.ReadValue<Vector2>();    // 어느방향으로 움직여야 하는지를 입력받음
            dir = inputdir;
            Debug.Log(inputdir);
        

        // Vector2 : 유니티에서 제공하는 구조체(struct). 2차원 백터를 표현하기 위한 구조체 (x,y)
        // Vector3 : 유니티에서 제공하는 구조체(struct). 3차원 백터를 표현하기 위한 구조체 (x,y, z)
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("발사");
        }
    }


    private void Test_OldInputManager()
    {
        //  input Manger를 이용한 입력처리
        //  Busy wait이 발생. (하는 일은 없지만 사용되고 있는 상태 => 전력 세이빙을 방해 => 전력소비 커짐)
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W가눌러졌따.");
            dir = Vector3.up;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A가눌러졌따.");
            dir = Vector3.left;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("S가눌러졌따.");
            dir = Vector3.down;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("D가눌러졌따.");
            dir = Vector3.right;
        }
    }
}
