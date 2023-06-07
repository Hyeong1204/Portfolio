using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMana
{
    float MP { get; set; }  // HP를 확인하고 설저할 수 있따.
    float MaxMP { get; }    // 최대HP를 확인할 수있다.

    /// <summary>
    /// HP가 변경될 때 실행될 델리게이트용 프로퍼티
    /// 파라메터는 현재 / 최대 비율
    /// </summary>
    Action<float> onManaChange { get; set; }

    /// <summary>
    /// 마나를 지속적으로 증가시켜주는 함수. 초당 totalRegen / duration 만큼씩 회복
    /// </summary>
    /// <param name="duration">전체 회복량</param>
    /// <param name="totalRenen">지속 시간</param>
    void ManaRegenerate(float totalRenen, float duration);
}
