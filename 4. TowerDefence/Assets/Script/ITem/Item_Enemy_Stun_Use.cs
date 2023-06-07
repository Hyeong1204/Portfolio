using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Enemy_Stun_Use : MonoBehaviour
{
    [Header("스턴 지속 시간")]
    public float stunTime = 5.0f;
    [Header("스턴 범위")]
    public float stunRange = 1.0f;
    public BuffType type;
    List<Enemy> enemies;

    BuffManager buffManager;

    SpriteRenderer sprite;
    Animator anim;
    CircleCollider2D col;
    Transform explosion;

    private void Awake()
    {
        buffManager = GameManager.Instance.Buff;
        col = GetComponent<CircleCollider2D>();
        col.radius = stunRange;
        anim = GetComponent<Animator>();
        enemies = new List<Enemy>(64);
        explosion = transform.GetChild(2);
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemies.Add(collision.GetComponent<Enemy>());
        }
    }

    public IEnumerator StartTimer()
    {
        anim.SetBool("TimerEnd", true);             // 애니메이션 작동
        yield return new WaitForSeconds(1.0f);      // 1초 뒤에
        anim.SetBool("TimerEnd", false);            // 애니메이션 중지
        col.enabled = false;                        // 콜라이더 끄기
        explosion.gameObject.SetActive(true);       // 폭발 애니메이션 겨기
        sprite.color = Color.clear;
        foreach (var enemy in enemies)
        {
            buffManager.CreateBuff(type, 0, stunTime, enemy);                 // 적한테 스턴 적용
        }
        
        Destroy(this.gameObject, 1.0f);             // 자신 삭제
    }
}
