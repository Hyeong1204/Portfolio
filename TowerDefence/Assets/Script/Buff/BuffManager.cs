using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public GameObject buffPrefab;           // 버프베이스 프리펩

    /// <summary>
    /// 버프 효과 적용(타워)
    /// </summary>
    /// <param name="type">버프 유형</param>
    /// <param name="value">버프의 증감량(% 유형)</param>
    /// <param name="time">지속시간</param>
    /// <param name="tower">버프 효과를 받을 타워</param>
    public void CreateBuff(BuffType type, float value, float time, Tower tower)
    {
        GameObject obj = Instantiate(buffPrefab, transform);
        obj.GetComponent<BuffBase>().Initialize(type, value, time, tower);
    }

    /// <summary>
    /// 버프 효과 적용(적)
    /// </summary>
    /// <param name="type">버프 유형</param>
    /// <param name="value">버프의 증감량(% 유형)</param>
    /// <param name="time">지속시간</param>
    /// <param name="tower">버프 효과를 받을 적</param>
    public void CreateBuff(BuffType type, float value, float time, Enemy enemy)
    {
        GameObject obj = Instantiate(buffPrefab, transform);
        obj.GetComponent<BuffBase>().Initialize(type, value, time, enemy);
    }
}
