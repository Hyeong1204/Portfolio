using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager instance;

    Queue<GameObject> enemyQueue = new Queue<GameObject>();
    [SerializeField]
    GameObject enemyPrefab;
    public Transform spawner;
    public int intcount = 5;

    private void Awake()
    {
        instance = this;
        Inialize(intcount);
    }

    private void Start()
    {
        for (int i = 0; i < intcount; i++)
        {
            GetObject();
        }
    }

    void Inialize(int intCount)
    {
        for (int i = 0; i < intCount; i++)
        {
            enemyQueue.Enqueue(CreateNewEnemy());
        }
    }

    GameObject GetObject()
    {
        if (enemyQueue.Count > 0)
        {
            var obj = enemyQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.transform.position = GetRandomPosition();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            return null;
        }
    }

    private Vector3 GetRandomPosition()
    {
        if (spawner == null)
        {
            return Vector3.zero;
        }

        Vector3 center = spawner.position;
        Vector3 scale = spawner.localScale;

        float randx = UnityEngine.Random.Range(center.x - scale.x *0.5f, center.x + scale.x *0.5f);
        float randz = UnityEngine.Random.Range(center.z - scale.z * 0.5f, center.z + scale.z * 0.5f); ;

        Vector3 randomPosition = new Vector3(randx, 1f, randz);

        return randomPosition;
    }

    GameObject CreateNewEnemy()
    {
        var newEnemy = Instantiate(enemyPrefab);
        newEnemy.SetActive(false);
        newEnemy.transform.SetParent(this.transform);

        return newEnemy;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(this.transform);
        enemyQueue.Enqueue(obj);

        Invoke("GetObject", 0.5f);
    }
}
