using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpwaner : MonoBehaviour
{
    public GameObject[] towers; // 타워를 배열로 받음
    public int[] towerCost = new int[] {100,200,300};
    public Action onTowerSetUp;


    public void SpawnTower(GameObject tileTransform,int index)
    {
        
        Tile tile = tileTransform.GetComponent<Tile>();

        if (!tile.isBulidTower && towerCost[index] < GameManager.Instance.Energy_Count)
        {

            Instantiate(towers[index], tileTransform.transform.position, Quaternion.identity);
            GameManager.Instance.Energy_Count -= towerCost[index];
            tile.isBulidTower = true;
            onTowerSetUp?.Invoke();
        }

    }
}
