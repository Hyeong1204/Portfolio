using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    Image icon;
    TextMeshProUGUI nameText;
    TextMeshProUGUI goldText;
    Dialogue dialogue;

    const int MaxItemCount = 100;

    int buyCount;
       
    bool isSold = false;

    public int goldValue;
    public ItemEnum itemEnum = ItemEnum.NULL;

    /// <summary>
    /// 남은 구매 수량
    /// </summary>
    public int RemainingCount => MaxItemCount - buyCount;

    public int BuyCount
    {
        get => buyCount;
        set
        {
            buyCount = value;
            if (buyCount >= MaxItemCount)
            {
                Sold();
            }
        }
    }

    public Action<ItemSlot> onSlotClick;

    private void Awake()
    {
        nameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        goldText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        icon = transform.GetChild(2).GetComponent<Image>();
        dialogue = FindObjectOfType<Dialogue>();
    }

    /// <summary>
    /// 해당 슬롯 세팅 및 초기화 (암시장 들어올 때 한번만 실행)
    /// </summary>
    /// <param name="code"></param>
    public void SlotInitialize(ItemEnum code, float ratio)
    {
        isSold = false;
        buyCount = 0;
        itemEnum = code;
        Inventory inven = Inventory.Inst;
        icon.sprite = inven.itemData[(int)code].itemIcon;
        nameText.text = inven.itemData[(int)code].itemName;
        goldValue = (int)(inven.itemData[(int)code].buyValue * ratio);
        goldText.text = $"{goldValue : #,0} G";        // 원가 가격에 ratio만큼 할인 된 가격
    }

    void Sold()
    {
        isSold = true;
        icon.color = new Color(1, 1, 1, 0.5f);
        nameText.text = $"<#ff0000>SOLD OUT</color>";
        goldText.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isSold)
        {
            onSlotClick?.Invoke(this);
            dialogue.NextDialogue(1);
        }
        else
        {
            dialogue.NextDialogue(4);
        }
    }
}
