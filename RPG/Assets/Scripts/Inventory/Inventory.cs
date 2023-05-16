using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<Item> items = new List<Item>();

    public int maxSpace = 9;

    public delegate void OnItemChagned();
    public OnItemChagned onItemChanged;

    private void Awake()
    {
        Instance = this;
    }

    public bool Add(Item addedItem)
    {
        if(items.Count >= maxSpace)
        {
            return false;
        }
        items.Add(addedItem);
        onItemChanged?.Invoke();

        return true;
    }

    public void Remove(Item addedItem)
    {
        items.Remove(addedItem);
        onItemChanged?.Invoke();
    }
}
