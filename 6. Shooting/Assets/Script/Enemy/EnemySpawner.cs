using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject SpawnObject;
    public GameObject item_Enemy;

    public float SpawnTime = 1.0f;
    protected float minY = -4.0f;
    protected float maxY = 4.0f;


    IEnumerator enemySpawn;



    // Start is called before the first frame update
    void Start()
    {
        enemySpawn = EnemySpawn();
        StartCoroutine(enemySpawn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual IEnumerator EnemySpawn()
    {
        while (true)
        {
            GameObject prefab = SpawnObject;                // 기본적으로 생성하는 것은 SpawnObject
            if(Random.Range(0.0f,1.0f) < 0.1f)                // 10% 확률로 적을 소환하는 로직
            {
                prefab = item_Enemy;                // 10% 이하의 확률로 item_Enemy 적 생성
            }
                                                           // 90% 확률로 일반 적을 소환
            GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);   // 생성하고 부모를 이 오브젝트로 설정
            obj.transform.Translate(0, Random.Range(minY, maxY), 0);        // 스폰 생성 범위 안에서 랜덤으로 높이 정하기
            
            yield return new WaitForSeconds(SpawnTime);     // SpawnTime 만큼 대기
        }
    }


    
    protected virtual void OnDrawGizmos()         // 개발용 정보를 항상 그리는 함수
    {
        //Gizmos.color = new Color(1, 0, 0);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new(1, Mathf.Abs(minY) + Mathf.Abs(maxY) + 2, 1));
    }

    //private void OnDrawGizmosSelected()     // 개발자 영역에서만 보이는 영역
    //{
        
    //}

}
