using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectWindow : MonoBehaviour
{
    CanvasGroup canvasGroup;
    BlackMarket shop;
    ItemSlot selectslot = null;

    // 선택된 아이템 창 출력할 것들 (이미지 텍스트 등등)
    Image selectImage;
    TextMeshProUGUI selectNameText;
    TextMeshProUGUI selectGoldText;
    // ----------------------------------------------------------

    Button buyButton;
    Button exitButton;

    Slider slider;
    TextMeshProUGUI sliderMaxText;

    TMP_InputField inputField;

    Dialogue dialogue;

    public Action<string> purchasefail;

    int itemCount;
    int ItemCount
    {
        get => itemCount;
        set
        {
            itemCount = value;
            if (selectslot != null)
            {
                selectGoldText.text = $"{itemCount * selectslot.goldValue :#,0} g"; 
            }
        }
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        shop = GetComponentInParent<BlackMarket>();
        // 선택된 아이템 창 출력할 것들 (이미지 텍스트 등등)
        selectImage = transform.GetChild(1).GetChild(3).GetComponent<Image>();
        selectNameText = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        selectGoldText = transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();
        // ----------------------------------------------------------

        buyButton = transform.GetChild(2).GetComponent<Button>();
        buyButton.onClick.AddListener(BuyItem);

        exitButton = transform.GetChild(3).GetComponent<Button>();
        exitButton.onClick.AddListener(Close);

        slider = GetComponentInChildren<Slider>();
        slider.onValueChanged.AddListener(SliderChangeValue);
        sliderMaxText = slider.transform.GetChild(4).GetComponent<TextMeshProUGUI>();

        inputField = GetComponentInChildren<TMP_InputField>();
        inputField.onValueChanged.AddListener(InputFieldValueChange);

        dialogue = FindObjectOfType<Dialogue>();


    }


    private void Start()
    {
        Close();
    }

    /// <summary>
    /// 구매 창 열기
    /// </summary>
    /// <param name="slot"></param>
    public void Open(ItemSlot slot)
    {
        selectslot = slot;
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        ItemData selectItemData = Inventory.Inst.itemData[(int)slot.itemEnum];

        selectImage.sprite = selectItemData.itemIcon;
        selectNameText.text = selectItemData.itemName;
        ItemCount = 1;

        int amount = slot.RemainingCount;           // 해당 슬롯의 남은 수량
        slider.minValue = 1;
        slider.value = 1;
        slider.maxValue = amount;
        sliderMaxText.text = $"{amount}(MAX)";
    }

    /// <summary>
    /// 구매창 닫기
    /// </summary>
    void Close()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.blocksRaycasts = false;
    }

    private void SliderChangeValue(float value)
    {
        inputField.text = ((int)value).ToString();
    }

    private void InputFieldValueChange(string text)
    {
        if (text != "")
        {
            int value = int.Parse(text);

            if (value <= selectslot.RemainingCount)
            {
                ItemCount = value;
                slider.value = value;
            }
            else
            {
                inputField.text = selectslot.RemainingCount.ToString();
            }
        }
    }

    /// <summary>
    /// 아이템 구매
    /// </summary>
    void BuyItem()
    {
        UI ui = UI.Instance;
        int buyGold = ItemCount * selectslot.goldValue;

        string text;
        if (ui.Gold >= buyGold)
        {
            selectslot.BuyCount += ItemCount;
            ui.Gold -= buyGold;
            Inventory.Inst.AddItem(ItemCount, selectslot.itemEnum);
            dialogue.NextDialogue(3);

            Close();
        }
        else
        {
            text = "골드가 부족합니다.";
            purchasefail?.Invoke(text);
            dialogue.NextDialogue(2);
        }
    }
}
