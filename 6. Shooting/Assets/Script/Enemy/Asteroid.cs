
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    GameObject explosion;
    public GameObject small;
    //[Range(1,16)]

    public int score = 50;
    int splitCount = 3;
    public float AsteriodSpeed = 3.0f;
    public float rotateSpeed = 360.0f;
    public float minMoveSpeed = 2.0f;
    public float maxMoveSpeed = 4.0f;
    public float minRotateSpeed = 30.0f;
    public float maxRotateSpeed = 360.0f;
    public float lifeTime = 3.0f;
    float timeset = 0.0f;

    public int Hp = 3;

    Action<int> onDead;

    //float X = -11.0f;
    //float maxY = 6.0f;
    //float minY = -6.0f;

    public Vector3 direction = Vector3.left;
    SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        timeset = UnityEngine.Random.Range(4.0f, 6.0f);
    }

    private void Start()
    {
        Player palyer = FindObjectOfType<Player>();
        onDead += palyer.AddScore;
        explosion = transform.GetChild(0).gameObject;
        AsteriodSpeed = UnityEngine.Random.Range(minMoveSpeed, maxMoveSpeed);                                   // 운석속도 = 최소값 ~ 최대값 사이에 값을 랜덤으로 넣어라
        float ratio = (AsteriodSpeed - minMoveSpeed) / (maxMoveSpeed - minMoveSpeed);               // ratio = (운석 속도 - 최소 운석속도) / (최대 운석속도 - 최소 운석속도) 비율 구하기
        rotateSpeed = ratio * (maxRotateSpeed - minRotateSpeed) + minRotateSpeed;                   // 최종 회전속도 = ratio *  (최대 회전속도 - 최소 회전속도) + 최소 회전속도
        

        int rand = UnityEngine.Random.Range(0, 4);                  // rand에다가 0 ~ 3 사이의 숫자를 랜덤으로 넣는다.
        sprite.flipX = ((rand & 0b_01) != 0);           // 0b_ = 이진수를 나타내는 것      첫번째 자리가 1이면 참
        sprite.flipY = ((rand & 0b_10) != 0);           // 두번째 자리가 1이면 참

        lifeTime = UnityEngine.Random.Range(3.0f, 5.0f);

        StartCoroutine(desAster());                     // 운석 n초 후에 소멸
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation *= Quaternion.Euler(new(0, 0, 90));      // 계속 90도씩 회전
        //transform.rotation *= Quaternion.Euler(new(0, 0, rotateSpped * Time.deltaTime));        // 1초에 360도씩 회전
        transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.forward);   // forward 축을 기주능로 1초에 rotateSpeed도씩 회전

        transform.Translate(Time.deltaTime * AsteriodSpeed * direction, Space.World);

        //if(transform.position.x < X || transform.position.y > maxY || transform.position.y < minY)
        //{
        //    Destroy(gameObject);
        //}

        //timeset += Time.deltaTime;
        //if(lifeTime <= timeset)
        //{ 
        //    Crush();
        //    timeset = 0.0f;
        //}
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            Hp--;
            if (Hp <= 0)
            {
                onDead?.Invoke(score);
                Crush();
            }
        }
    }


    void Crush()
    {
        explosion.SetActive(true);
        explosion.transform.parent = null;


        if(UnityEngine.Random.Range(0.0f,1.0f) < 0.05f)
        {
            // 5% 확률에 당첨 되었다.
            splitCount = 20;
        }
        else
        {
            // 95% 확률에 담첨 되었다.
            splitCount = UnityEngine.Random.Range(3, 10);                   // 운석 갯수 랜덤
        }
        float angleGap = 360.0f / (float)splitCount;        // 운석 개수 만큼 사이각을 계산
        float anglstemp = UnityEngine.Random.Range(0.0f, 360.0f);       // 첫 운석 각도 랜덤
        for(int i = 0; i < splitCount; i++)
        {
            Instantiate(small, transform.position, Quaternion.Euler(0,0,((angleGap * i ) + anglstemp)));
        }

        Destroy(this.gameObject);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + direction * 1.5f);
    }


    IEnumerator desAster()              // 운석이 n초 뒤에 소멸하는 코루틴
    {
        

        yield return new WaitForSeconds(timeset);
        Crush();
    }
}
