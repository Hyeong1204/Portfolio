using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemCount = 0;

    bool isSetting = false;

    ItemEnum itemType;


    public ItemEnum ItemType
    {
        get => itemType;
        set
        {
            if (!isSetting)
            {
                isSetting = true;
                itemType = value;
            }
        }
    }

    private void Start()
    {
        itemCount = 0;
    }
}
