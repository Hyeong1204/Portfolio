using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefab;      //  enemy 프리펩

    public float spawnTime;             // 스폰 시간

    public Transform[] wayPoints;       // 현재 스테이지 이동 경로

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.Player_HP = gameManager.Player_HP_Max;        // 플레이어 HP를 최대치로 설정

        gameManager.MaxRound = enemyPrefab.Length;
        gameManager.roundUp += StartSpawnEnemy;
        StartCoroutine(SpawnEnemy());   // SpawnEnemy코루틴 시작
    }

    private void OnDisable()
    {
        gameManager.roundUp -= StartSpawnEnemy; 
    }

    void StartSpawnEnemy()
    {
        if (enemyPrefab.Length > GameManager.Instance.Round)
        {
            StartCoroutine(SpawnEnemy());
        }
    }

    IEnumerator SpawnEnemy()
    {
        if (enemyPrefab[GameManager.Instance.Round].GetComponent<Enemy_Spawn_Count_Up>() != null)
        {
            GameManager.Instance.Enemy_Spawn_Count *= 2;
        }
        else if (enemyPrefab[GameManager.Instance.Round].GetComponent<Enemy_Spawn_Count_Down>() != null)
        {
            GameManager.Instance.Enemy_Spawn_Count -= 5;
        }
        else if (enemyPrefab[GameManager.Instance.Round].GetComponent<Enemy_Spawn_Count_Boss>() != null)
        {
            GameManager.Instance.Enemy_Spawn_Count = 1;
        }
        else
        {
            GameManager.Instance.Enemy_Spawn_Count = 10;
        }
        for ( int i=0; i< GameManager.Instance.Enemy_Spawn_Count; i++)
        {
            GameObject obj = Instantiate(enemyPrefab[GameManager.Instance.Round]);  // obj에 enemyPrefab오브젝트 생성
            Enemy enemy = obj.GetComponent<Enemy>();        // obj에 생성된 적의 enemy 컴포넌트

            enemy.Setup(wayPoints);         // wayPoints 정보를 매개변수로 Setup() 호출

            yield return new WaitForSeconds(spawnTime);     // spawnTime 시간 동안 대기
        }
    }

}
