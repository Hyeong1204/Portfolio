
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner_ : EnemySpawner
{

    Transform destination;


    private void Awake()
    {
        // 오브젝트가 생성된 직후 =>  이 오브젝트 안에 잇는 것들을 초기화 할 때
        // 이 오브젝트안에 있는 모든 컴포넌트가 생성이 완료되었다.
        // 그리고 이 오브젝트의 자식 오브젝트들도 모드 생성이 완료 되었다.

        //destination = transform.Find("DestinationArea");    // DestinationArea라는 이름을 가진 자식 찾기
        destination = transform.GetChild(0);    // 첫번째 자식 찾기
    }



    // Start is called before the first frame update
    //void Start()
    //{
    //    // 첫번째 업데이트 실행 직전 호출
    //    // 나와 다른 오브젝트를 가져와야 할 때 사용
    //}

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    protected override IEnumerator EnemySpawn()
    {
        while (true)
        {
            GameObject obj = Instantiate(SpawnObject, transform.position, Quaternion.identity);   // 생성하고 부모를 이 오브젝트로 설정
            obj.transform.Translate(0, Random.Range(minY, maxY), 0);        // 스폰 생성 범위 안에서 랜덤으로 높이 정하기

            Vector3 destPossiton = destination.position + new Vector3(0.0f, Random.Range(minY, maxY), 0);       // 목적지 위치

            Asteroid asteroid = obj.GetComponent<Asteroid>();               // obj에 있는 컴퍼먼트(Asteroid)을 가져온다. 만약 Asteroid가 없다면 null값이 들어간다.
            if (asteroid != null)       // 값이 null이 아니면 
            {
                asteroid.direction = (destPossiton - asteroid.transform.position).normalized;       // normalized 크기를 항상 1로 만들어줌
                                      // destPossiton - asteroid.transform.position 이렇게만 쓰면 힘과 크기가 같이 있기 때문에 이동속도가 빠르다
            }

            yield return new WaitForSeconds(SpawnTime);     // SpawnTime 만큼 대기
        }
    }


    
    protected override void OnDrawGizmos()         // 개발용 정보를 항상 그리는 함수
    {
        //Gizmos.color = new Color(1, 0, 0);
        Gizmos.color = Color.red;           // 기지모 색 바꾸기
        Gizmos.DrawWireCube(transform.position, new(1, Mathf.Abs(minY) + Mathf.Abs(maxY) + 2, 1));

        if (destination == null)
        {
            destination = transform.GetChild(0);
        }
            Gizmos.DrawWireCube(destination.position, new(1, Mathf.Abs(minY) + Mathf.Abs(maxY) + 2, 1));
    }

    //private void OnDrawGizmosSelected()     // 개발자 영역에서만 보이는 영역
    //{
        
    //}
    
}
