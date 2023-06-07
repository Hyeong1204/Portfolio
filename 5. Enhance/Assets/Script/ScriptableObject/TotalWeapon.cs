using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "ScriptableObject/TotalWeaponData", order = 2)]
public class TotalWeapon : ScriptableObject
{
    public int itemID;

    public WeaponData[] WeaponDatas;
}
