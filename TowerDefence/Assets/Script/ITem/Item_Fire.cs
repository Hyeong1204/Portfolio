using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Fire : MonoBehaviour
{
    Transform fire;
    SpriteRenderer sprite;

    [Header("초당 공격력")]
    public int fireDamge = 25;                 // 초당 데미지
    [Header("지속 시간")]
    public float itemDestroyTime = 5.0f;       // 아이템 디스폰 시간 (아이템 발동 후)
    float alpha = 1;                    // sprite 알파 값
    bool onePlay = true;                // 한 번만 실행 (false면 실행함)
    bool spriteAlpha = false;           // 알파 값조절 (true면 알파값 조절 시작)

    private void Awake()
    {
        fire = transform.GetChild(0);
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        onePlay = true;
        spriteAlpha = false;
        fire.gameObject.SetActive(false);       // 오브젝트 비활성화
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (onePlay)
            {
                fire.gameObject.SetActive(true);        // 오브젝트 활성화
                onePlay = false;                        // 한번만 실행
                sprite.color = Color.clear;             // 함정 위치 sprite 끄기
                Destroy(this.gameObject, itemDestroyTime);
                StartCoroutine(SetAlphaDlray());        // 코루틴 시작(알파 값 딜레이)
            }
            var enemy = collision.GetComponent<Enemy>();
            enemy.FireUse(fireDamge);
        }
    }

    private void Update()
    {
        if (spriteAlpha)            // 알파값 줄이기 (천천히 사라지는 애니메이션)
        {
            alpha -= Time.deltaTime;        // 초당 1씩 감소
            sprite.color = new Color(1, 1, 1, alpha);   // 알파값 재설정
            if(alpha < 0.0f)        // 알파 값이 0보다 작으면
            {
                spriteAlpha = false;        // 알파값 실행 중지
            }
        }
    }

    IEnumerator SetAlphaDlray()         // 알파 값 딜레이 코루틴
    {
        sprite = fire.GetComponent<SpriteRenderer>();
        yield return new WaitForSeconds(itemDestroyTime - 1.0f);
        spriteAlpha = true;                 // 알파값 조절 실행
    }
}
