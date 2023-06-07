using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    Item item;

    public Image icon;
    public Button xButton;

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        xButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        xButton.interactable = false;
    }

    public void OnXbuttonClick()
    {
        Inventory.Instance.Remove(item);
    }

    public void OnItemButtonClock()
    {
        if(item != null)
        {
            item.Use();
        }
    }
}
