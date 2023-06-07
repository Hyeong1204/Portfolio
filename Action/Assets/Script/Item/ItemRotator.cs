using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 스크립트를가진 오브젝트는 y축을 기준으로 계속 회전(시계 방향)하면서 위아래로 올라갔다 내려갔다 한다(삼각함수 활용).
public class ItemRotator : MonoBehaviour
{
    public float rotateSpeed;       // 오브젝트의 회전 속도
    public float minHeight;         // 오브젝트의 가장 낮은 높이
    public float maxHeight;         // 오브젝트의 가장 높은 높이
    //public float moveSpeed;         // y축 기준으로 위래 왕복운동 속도

    float runningTime;              // 전체 실행 시간(cos에 사용할 용도)
    float halfDiff;                 // 계산 캐싱용

    Vector3 newPosition;            // 아이템의 새로운 위치

    private void Start()
    {
        newPosition = transform.position;       // 현재 위치로 newPosition을 설정
        newPosition.y = minHeight;              // newPosition의 y 값을 가장 낮은 높이값으로 설정
        transform.position = newPosition;       // 오브젝트 위치를 newPosition으로 설정
        runningTime = 0.0f;                     // 시간 누적값 초기화 
        halfDiff = 0.5f * (maxHeight - minHeight);  // 캐싱용 계산결과 저장
    }

    private void Update()
    {
        transform.Rotate(0, Time.deltaTime * rotateSpeed * 1.0f, 0);        // 제자리에서 빙글빙글 돌리기

        runningTime += Time.deltaTime;          // 시간 계속누적 ( 한번 왕복하는데 3.141592..........초만큼 이동)
        newPosition.x = transform.parent.position.x;            // 보모의 x,z 위치는 계속적용
        newPosition.z = transform.parent.position.z;
        newPosition.y = minHeight + (1 - Mathf.Cos(runningTime)) * halfDiff; // 높이값은 cos 그래프를 이용해서 계산
        transform.position = newPosition;       // 계산이 끄난 newPostion으로 위치 옮기기
        //runningTime += Time.deltaTime * moveSpeed;                          // 1초에 1 x moveSpeed 만큼 증가
        //yPos = Mathf.Clamp(Mathf.Sin(runningTime), minHeight, maxHeight);   // 사인 함수를 이용하여 -1 ~ 1 사이 값이 나오고 Clamp 함수를 이용하여 최소 높이와 최대높이를 만든다.
        //transform.Translate(Time.deltaTime * yPos * Vector3.up);            // y축 기준으로 yPos값만큼 이동
    }
}