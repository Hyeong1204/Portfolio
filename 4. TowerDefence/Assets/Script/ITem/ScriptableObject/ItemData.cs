using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Item Data", order = 1)]
public class ItemData : ScriptableObject
{
    public uint id = 0;                     // 아이템 ID
    public string itemName = "아이템";      // 아이템 이름
    public GameObject modelprefab;         // 아이템 모델 프리펩
    [Header("아이템 가격")]
    public uint itemCost;                  // 아이템의 가격
    [Header("아이템 수량")]
    public uint count = 0;                 // 소유중인 아이템 갯수
    [Header("설치가능한 타일")]
    public string[] layerNames = null;     // 설치 가능한 레이어
    [Header("UI에 표시될 아이콘")]
    public Sprite itemIcon;

    [Header("아이템 설명")]
    [TextArea(3,5)]
    public string description = string.Empty;   // 아이템 설명
}
