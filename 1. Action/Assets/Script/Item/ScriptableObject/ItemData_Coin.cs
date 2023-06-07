using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Coin Item Data", menuName = "Scriptable Object/Item Data - Coin", order = 4)]
public class ItemData_Coin : ItemData, IConsumable
{
    public void Consume(GameObject target)
    {
        Player player = target.GetComponent<Player>();
        player.Money += (int)value;
    }
}
