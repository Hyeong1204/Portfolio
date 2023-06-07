using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Ui : MonoBehaviour
{
    Item_Base[] itemBase;
    private void Awake()
    {
        itemBase = GetComponentsInChildren<Item_Base>();
    }

    private void Start()
    {
        for (int i = 0; i < itemBase.Length; i++)
        {
            itemBase[i].ItemInitialize((uint)i);
            itemBase[i].Refresh();
        }

    }
}