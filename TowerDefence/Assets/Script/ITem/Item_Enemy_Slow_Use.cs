using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item_Enemy_Slow_Use : MonoBehaviour
{
    [Header("버프 지속 시간")]
    public float time = 0.0f;
    [Header("이동속도 감소량")]
    [Range(0.0f, 1.0f)]
    public float speedSlow = 0.0f;

    public BuffType type;

    void Start()
    {
        Destroy(this.gameObject, time+1.0f);
        StartCoroutine(ItemDestory());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // 슬로우 효과 적용
            Movement enemy = collision.GetComponent<Movement>();
            enemy.MoveSpeed = enemy.OriginalMoveSpeed * speedSlow;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // 슬로우 효과 해제
            Movement enemy = collision.GetComponent<Movement>();
            enemy.MoveSpeed = enemy.OriginalMoveSpeed;
        }
    }

    /// <summary>
    /// 지속시간이 끝나면 없애는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator ItemDestory()
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
