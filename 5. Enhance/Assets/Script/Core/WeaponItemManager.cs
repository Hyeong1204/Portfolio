using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemManager : MonoBehaviour
{
    public TotalWeapon[] weaponItems;

    public TotalWeapon this[int id] => weaponItems[id];

    public TotalWeapon this[WeaponEnum code] => weaponItems[(int)code];

}