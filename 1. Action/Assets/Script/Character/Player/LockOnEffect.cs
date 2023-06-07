using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class LockOnEffect : MonoBehaviour
{
    IHealth TargetHealth;

    /// <summary>
    /// 락온 대상 설정
    /// </summary>
    /// <param name="newParent">새로운 락온 대상(무조건 Enemy이어야 한다.). null이면 끄기</param>
    public void SetLockOnTarget(Transform newParent)
    {
        if(TargetHealth != null)    // 기존의 대상이 있었으면
        {
            TargetHealth.onDie -= ReleasTarget;     // 연결되어있던 델리게이트 등록 해제
        }

        if (newParent != null)
        {
            TargetHealth = newParent.gameObject.GetComponent<IHealth>();        // 소롭게 대상이 설정
            TargetHealth.onDie += ReleasTarget;                                 // 죽을 때 이펙트 제거하도록 함수 등록
        }

        transform.SetParent(newParent);                     // 부모 설정
        transform.localPosition = Vector3.zero;             // 부모 위치로 이동
        this.gameObject.SetActive(newParent != null);       // newParnt가 있으면 true, null이면 false
    }

    void ReleasTarget()
    {
        SetLockOnTarget(null);
    }
}
