using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 0)]
public class Item : ScriptableObject
{
    public string itemName = "Item";
    public Sprite icon = null;
    public AudioClip audioClip = null;

    public virtual void Use()
    {
        //Debug.Log("아이템 사용");
        RemoveFormInventory();
    }

    public void RemoveFormInventory()
    {
        Inventory.Instance.Remove(this);
    }
}
