using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Button_Click_Item : MonoBehaviour
{
    public GameObject buy_panel;

    public ItemData itemData;

    public ItemData ItemData
    {
        get => itemData;
    }

    Image itemImage;
    TextMeshProUGUI itemPrice;

    private void Awake()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
        itemPrice = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        if (ItemData == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            transform.name = ItemData.name;
            itemImage.sprite = ItemData.itemIcon;
            itemPrice.text = ItemData.itemCost.ToString();
        }
    }

    public void Open_Buy_Panel()
    {
        buy_panel.SetActive(true);
        buy_panel.GetComponent<Button_Buy_Item>().Display_ShopWindow(ItemData);
    }
}
