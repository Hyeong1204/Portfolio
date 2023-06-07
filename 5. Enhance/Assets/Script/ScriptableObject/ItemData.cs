using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "ScriptableObject/ItemData", order = 0)]
public class ItemData : ScriptableObject
{
    [Header("아이템 기본 데이터")]
    public int itemID;
    public string itemName;
    public GameObject itemPrefab;
    public Sprite itemIcon;
    [Header("아이템 구매 비용")]
    public int buyValue;
    [Header("아이템 판매 비율")]
    [Range(0f, 1f)]
    public float saleRatio;
    public int SaleValue
    {
        get => (int)(buyValue * saleRatio);       // 무조건소수점 잘라냄
    }

}
