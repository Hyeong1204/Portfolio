using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower Data", menuName = "Scriptable Object/Tower Data", order = 2)]
public class TowerData : ScriptableObject
{
    public uint id = 0;
    public string towerName = "타워 이름";      // 타워 이름
    public GameObject modelprefab;         // 타워 모델 프리펩
    [Header("타워 가격")]
    public uint towerCost;                  // 타워의 가격
    [Header("UI에 표시될 아이콘")]
    public Sprite towerIcon;

    [Header("타워 설명")]
    [TextArea(3,5)]
    public string description = string.Empty;   // 아이템 설명
}
