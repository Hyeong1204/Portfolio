using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Item_Tower_PowerUp_Use : MonoBehaviour
{
    [Header("지속 시간")]
    public float time = 5.0f;
    [Header("공격력 증가량")]
    [Range(1.0f, 3.0f)]
    public float power = 1.0f;
    public BuffType tpye;
    BuffManager buffMananger;

    WaitForSeconds seconds;
    Transform spriteTransform;           // 자식 이미지

    private void Awake()
    {
        spriteTransform = transform.GetChild(0);
    }

    private void Start()
    {
        buffMananger = GameManager.Instance.Buff;
        seconds = new WaitForSeconds(0.02f);
        StartCoroutine(Rotate());                                   // 회전 코루틴 시작
        Destroy(this.gameObject, time);                             // time초 후에 오브젝트 제거
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tower"))
        {
            Tower tower = collision.GetComponent<Tower>();            
            buffMananger.CreateBuff(tpye, power, time, tower);      // 타워에게 해당 버프 적용
            //Debug.Log(tower.gameObject.name);
        }
    }

    IEnumerator Rotate()
    {
        while (true)
        {
            spriteTransform.Rotate(-Vector3.forward);
            yield return seconds;
        }
    }
}
