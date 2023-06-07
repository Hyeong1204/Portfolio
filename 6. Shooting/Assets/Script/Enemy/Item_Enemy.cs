using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Enemy : MonoBehaviour
{
    public GameObject Item;         // item_Enemy에 붙어 있는 파워업 아이템
    public int score = 15;
    Action<int> onDead;

    private void Start()
    {
        Player palyer = FindObjectOfType<Player>();
        onDead += palyer.AddScore;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 블릿에 맞으면 실행
        if (collision.gameObject.CompareTag("Bullet"))
        {
            onDead?.Invoke(score);
            Instantiate(Item, transform.position, Quaternion.Euler(0,0,90.0f));
        }
    }
}
