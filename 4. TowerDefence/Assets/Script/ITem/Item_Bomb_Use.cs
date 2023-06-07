using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class Item_Bomb_Use : MonoBehaviour
{
    public Action onItemUse;        // 델리게이트
    Transform explostion;
    SpriteRenderer sprite;

    private void Awake()
    {
        explostion = transform.GetChild(0);
        sprite = GetComponent<SpriteRenderer>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ObstacleTile"))   // 장애물과 만나면
        {
            StartCoroutine(ExplosionStart());          // 코루틴 시작
        }
    }

    IEnumerator ExplosionStart()
    {
        yield return new WaitForSeconds(4.0f);
        StartCoroutine(Distroy());
        sprite.color = Color.clear;
        explostion.gameObject.SetActive(true);
        onItemUse?.Invoke();                // Tile_Obstacle로 델리게이트 보냄
    }

    IEnumerator Distroy()
    {
        yield return new WaitForSeconds(1.0f);      // 2초 후에 (라운드 시스템 들어가면 라운드마다 터지게 바꾸기)

        Destroy(this.gameObject);           // 자기 자신 삭제하기
    }
}
