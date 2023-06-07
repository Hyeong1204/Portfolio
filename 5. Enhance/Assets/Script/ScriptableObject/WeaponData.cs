using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "New Item Data", menuName = "ScriptableObject/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    [Serializable]
    public struct CostData
    {
        public ItemEnum costItem;       // 소비할 아이템
        public int costItemValue;       // 아이템의 갯수
    }

    [Header("무기 기본 데이터")]
    public int itemID;
    public string itemName;
    public GameObject itemPrefab;
    public Sprite itemIcon;
    [Header("무기 아이템 판매 비용")]
    public int saleValue;

    [Header("판매시 획득 명성치")]
    public int fameValue;

    // 강화 비용
    [Header("무기 강화 비용 (돈)")]
    public int costMoneyValue;

    [Header("무기 강화 비용 (재료 아이템)")]
    public CostData[] costData;

    // 강화 확률
    [Header("무기 강화 확률")]
    [Range(0f, 1f)]
    public float enforceRatio;

    [Header("무기 파괴 확률")]
    [Range(0f, 1f)]
    public float destroy;

    [Header("하락 및 성공시 바뀔 아이템 (0은 하락, 1은 성공)")]
    public WeaponData[] changeWeaponData;       // 하락 및 강화성공 후 나올아이템 

    public float DropRatio
    {
        get => 1 - enforceRatio - destroy;
    }

    public float DestroyRatio => enforceRatio + destroy;
}
