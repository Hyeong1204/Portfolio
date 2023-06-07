using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Move_Enemy : MonoBehaviour
{
    GameObject explosion;


    public int score = 10;
    public float speed = 3.0f;
    float timeElapsed;              // 게임 시작부터 얼마나 시간이 지났나를 기록해 놓는 변수
    float spawnY = 1.0f;            // 생성 되었을 때 기준 높이

    public float amplutude = 2.0f;        // 사인으로 변경되는 위 아래 차이 원래 sin -1 ~ 1 인데 그걸 변경하는 변수
    public float frequency = 1.0f;     // 사인 그래프가 한번도 도는데 걸리는 신간(원래는 2파이)
    Action<int> onDead;

    // Start is called before the first frame update
    void Start()
    {
        Player palyer = FindObjectOfType<Player>();
        onDead += palyer.AddScore;
        explosion = transform.GetChild(0).gameObject;
        //explosion.SetActive(false); // 활성화 상태를 끄기(비활성화)
        spawnY = transform.position.y;
        timeElapsed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Time.deltaTime : 이전 프레임에서 현재 프레임까지의 시간
        timeElapsed += Time.deltaTime * frequency;                         // 폭을 곱해주면 sin파의 간격이 줄어든다

        // Mathf.Sin(timeElapsed)구한 값에 heightDiff를 곱하면 sin파의 높이가 달라진다.
        float newY = spawnY + Mathf.Sin(timeElapsed) * amplutude;             // 결과는 0에서 시작해서 +1까지 증가 하다가 -1까지 감소. 디시 +1까지 증가
        float newX = transform.position.x - speed * Time.deltaTime;

        transform.position = new Vector3(newX, newY);

        //transform.Translate(speed * Time.deltaTime * Vector3.left, Space.World);
        //transform.Translate(Time.deltaTime * speed * Vector3.left, Space.Self);
        //if(transform.position.x < dis.x)
        //{
        //    Destroy(gameObject);
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            onDead?.Invoke(score);
            //GameObject obj = Instantiate(explosion, collision.transform.position, Quaternion.identity);
            //Destroy(obj, 0.42f);
            explosion.SetActive(true);  // 총알에 맞았을 때 비활성화를 활성화로 만듬
            explosion.transform.parent = null;      // 익스플로전의 부모(Enemy) 연결을 제거한다.
            Destroy(this.gameObject);   // Enemy를 파괴한다.
        }
    }
}
