using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Button_Buy_Item : MonoBehaviour
{
    GameManager gameManager;
    ItemData buy_item;
    GameObject panel_buy;
    GameObject window_buy;
    TextMeshProUGUI text_item_name;
    TextMeshProUGUI text_item_price;
    TextMeshProUGUI text_item_script;
    TextMeshProUGUI text_item_count;

    Button button_buy;
    Button button_cancle;

    private void Awake()
    {
        panel_buy = transform.gameObject;
        window_buy = transform.GetChild(1).gameObject;

        text_item_name = window_buy.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        text_item_price = window_buy.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        text_item_script = window_buy.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        text_item_count = window_buy.transform.GetChild(4).GetComponent<TextMeshProUGUI>();

        button_buy = window_buy.transform.GetChild(5).GetComponent<Button>();
        button_cancle = window_buy.transform.GetChild(6).GetComponent<Button>();

        button_buy.onClick.AddListener(Buy_Item);
        button_cancle.onClick.AddListener(Buy_Cancle);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void Display_ShopWindow(ItemData data)
    {
        buy_item = data;
        text_item_name.text = buy_item.itemName;
        text_item_price.text = buy_item.itemCost.ToString();
        text_item_script.text = buy_item.description;
        text_item_count.text = $"보유 수 : {buy_item.count}";
    }

    void Buy_Item()
    {
        gameManager.Money_Count -= (int)buy_item.itemCost;
        buy_item.count++;
        panel_buy.SetActive(false);
    }

    void Buy_Cancle()
    {
        panel_buy.SetActive(false);
    }
}
