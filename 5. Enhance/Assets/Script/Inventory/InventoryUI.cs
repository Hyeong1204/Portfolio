using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    CanvasGroup canvasGroup;

    Button closeButton;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        closeButton = GetComponentInChildren<Button>();
        closeButton.onClick.AddListener(InventoryClose);
    }

    private void Start()
    {
        InventoryClose();
    }

    public void InventoryOpen()
    {
        int index = transform.GetChild(1).childCount;

        Transform slot;

        Inventory inven = Inventory.Inst;

        for (int i = 0; i < index; i++)
        {
            slot = transform.GetChild(1).GetChild(i);
            slot.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{inven.itemData[i].itemName}";
            slot.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{inven.inventory[i].itemCount : #,0} 개";
            slot.GetChild(2).GetComponent<Image>().sprite = inven.itemData[i].itemIcon;
        }

        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
    }

    public void InventoryClose()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.blocksRaycasts = false;
    }
}
