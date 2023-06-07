using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUP_Move : MonoBehaviour
{
    float speed = 3.0f;                 // 이동 속도
    float coolTime = 5.0f;              // 파워업 아이템의 이동방향이 바뀌는데 걸리는 시간.
    Vector2 move;                       // 현재 이동 방향

    Player player;                      // 파워업 아이템의 이동방향 설정에 필요한 플레이어

    private void Awake()
    {
        player = FindObjectOfType<Player>();        //player 타입을 찾기(무조건 한개만 찾기 때문에 여러개가 있을 경우 어떤 것이 들어올지는 알 수 없다.)
    }

    private void Start()
    {
        SetRandomDir();                             // 랜덤하게 현재 이동 방향 설정
        StartCoroutine(DirChange());                // 코루틴 실행해서 일정 시간 간격으로 이동 방향 변경되게 설정
        Destroy(this.gameObject, 30.0f);            // 10초 뒤에 소멸
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * move);     // 현재 이동방향으로 초당 speed만큼 이동하기
    }


   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            // 보더랑 충돌하면 dir 반사
            move = -Vector2.Reflect(move, collision.contacts[0].normal);
        }
    }

    /// <summary>
    /// 이동 방항을 주기적으로 변경하는 코루틴용 함수.
    /// </summary>
    /// <returns></returns>
    IEnumerator DirChange()
    {
        while (true)        // 무한 반복
        {
            yield return new WaitForSeconds(coolTime);  //coolTime 만큼 대기
            SetRandomDir(false);                        // 랜덤하게 현재 이동 방향을 변경
        }
    }

    /// <summary>
    /// 랜덤하게 현재 이동방향 설정
    /// </summary>
    /// <param name="allRandom">true면 완전 랜덤, false면 플레이어 반대방향으로 이동할 확률이 높다.</param>
    void SetRandomDir(bool allRandom = true)            // 디폴트 파라메터. 값을 지정하지 않으면 디폴트 값이 대신 들어간다.
    {
        if (allRandom)
        {
        move = Random.insideUnitCircle;             // 반지름 1인 원 안의 랜덤한 위치 리턴 => 이 원의 원점에서 랜덤한 위치로 가는 방향 백터 생성
        move = move.normalized;                     //
        }
        else
        {
            Vector2 playerToPowerUp = transform.position - player.transform.position;           // 플레이어 위치에서 파워업 아이템 위치로 가는 방향백터 계산.
            playerToPowerUp = playerToPowerUp.normalized;                                       // 단위 백터로 변경
            if(Random.value < 0.6f)         // 60% 확률로 플레이어 반대방향으로 이동
            {
                move = Quaternion.Euler(0, 0, Random.Range(-90.0f, 90.0f)) * playerToPowerUp;           // playerToPowerUp 백터를 z축으로 -90~+90만큼 회천시켜서 dir에 넣기
            }
            else   //  40%확률로 플레이어 방향으로 이동
            {
                move = Quaternion.Euler(0, 0, Random.Range(-90.0f, 90.0f)) * -playerToPowerUp;
            }
        }
    }

}
