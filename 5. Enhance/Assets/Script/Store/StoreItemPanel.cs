using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreItemPanel : MonoBehaviour
{
    WeaponSalePanel weaponSalePanel;
    //Inventory inventory;
    StoreItemSlot[] itemSlots;
    StoreItemSlot itemSlot;

    Image selectSlotItemIcon;
    TextMeshProUGUI selectSlotItemName;
    TextMeshProUGUI selectSlotItemPrice;

    Button weaponSale;
    Button buyButton;
    Button saleButton;
    Button exitButton;

    Slider selectSlotSlider;
    TMP_InputField selectSlotInputField;
    TextMeshProUGUI selectSlotMinText;
    TextMeshProUGUI selectSlotInputFieldText;

    Image selectImage2;
    TextMeshProUGUI informationText;
    TextMeshProUGUI possessionitem;
    TextMeshProUGUI itemPriceText;
    Button yesButton;
    Button noButton;

    Image informationImage;
    TextMeshProUGUI popUpwindow;
    Button popupButton;

    UI ui;

    //int itemCount;
    int itemCountMin = 1;
    int itemCountMax = 999;

    int selectIndex;

    bool isBuy;    

    private void Awake()
    {
        weaponSalePanel = FindObjectOfType<WeaponSalePanel>();
        //inventory = FindObjectOfType<Inventory>();
        itemSlots = GetComponentsInChildren<StoreItemSlot>();
        itemSlot = GetComponentInChildren<StoreItemSlot>();

        weaponSale = transform.GetChild(10).GetComponent<Button>();
        weaponSale.onClick.AddListener(() => weaponSalePanel.WeaponSalePanelOnOff(true));

        Transform select = transform.GetChild(11).GetChild(0).GetChild(0);
        selectSlotItemName = select.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        selectSlotItemPrice = select.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        selectSlotItemIcon = select.GetChild(0).GetChild(3).GetComponent<Image>();

        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].onStoreItemSlot += onSelectItemSlot;            
        }

        buyButton = select.GetChild(1).GetComponent<Button>();
        buyButton.onClick.AddListener(BuyStoreItem);

        saleButton = select.GetChild(2).GetComponent<Button>();
        saleButton.onClick.AddListener(SaleStoreItem);

        exitButton = select.GetChild(4).GetComponent<Button>();
        exitButton.onClick.AddListener(itemSlotActive);

        selectSlotSlider = select.GetChild(3).GetComponent<Slider>();
        selectSlotInputField = select.GetChild(3).GetChild(5).GetComponent<TMP_InputField>();
        selectSlotInputFieldText = select.GetChild(3).GetChild(5).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        selectSlotMinText = select.GetChild(3).GetChild(3).GetComponent<TextMeshProUGUI>();

        select = transform.GetChild(12);
        selectImage2 = select.GetComponent<Image>();
        informationText = select.GetChild(0).GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        possessionitem = select.GetChild(0).GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>();
        itemPriceText = select.GetChild(0).GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>();

        yesButton = select.GetChild(0).GetChild(0).GetChild(5).GetComponent<Button>();
        yesButton.onClick.AddListener(YesButtonFunction);
        noButton = select.GetChild(0).GetChild(0).GetChild(6).GetComponent<Button>();
        noButton.onClick.AddListener(() => NoButtonFunction());

        select = transform.GetChild(13);
        informationImage = select.GetComponent<Image>();
        popUpwindow = select.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        popupButton = select.GetChild(0).GetChild(0).GetChild(2).GetComponent<Button>();
        popupButton.onClick.AddListener(() => informationImage.gameObject.SetActive(false));

        ui = FindObjectOfType<UI>();
    }

    private void Start()
    {
        selectImage2.gameObject.SetActive(false);
        informationImage.gameObject.SetActive(false);

        for (int i = 0; i < Inventory.Inst.itemData.Length; i++)
        {
            itemSlots[i].ItemName.text = Inventory.Inst.itemData[i].itemName;
            itemSlots[i].ItemPrice.text = Inventory.Inst.itemData[i].buyValue.ToString("#,0") + " G";
            
        }

        selectSlotSlider.maxValue = itemCountMax;
        selectSlotSlider.minValue = itemCountMin;
        selectSlotSlider.onValueChanged.AddListener(SliderValueChange);

        selectSlotInputField.onValueChanged.AddListener(OninputChange);

    }    

    void onSelectItemSlot(StoreItemSlot slot)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i] == slot)
            {
                selectIndex = i;
                selectSlotItemName.text = Inventory.Inst.itemData[i].itemName;
                selectSlotItemPrice.text = Inventory.Inst.itemData[i].buyValue.ToString("#,0") + " G";
                selectSlotItemIcon.sprite = Inventory.Inst.itemData[i].itemIcon;
            }
        }
    }

    void SliderValueChange(float value)
    {        
        selectSlotSlider.value = Mathf.Clamp(value, itemCountMin, itemCountMax);
        selectSlotMinText.text = selectSlotSlider.value.ToString("F0");
        selectSlotInputField.text = ((int)selectSlotSlider.value).ToString();
        //selectSlotItemPrice.text = ((int)selectSlotSlider.value * Inventory.Inst.itemData[selectIndex].buyValue).ToString() + "G";
    }

    private void OninputChange(string text)
    {
        if (text != "")
        {
            if (int.Parse(text) >= itemCountMin && int.Parse(text) <= itemCountMax)
            {
                selectSlotInputField.text = text;
                selectSlotSlider.value = int.Parse(text);
                Debug.Log($"{text}를 입력함");
            }
            else if (int.Parse(text) < itemCountMin)
            {
                selectSlotInputField.text = itemCountMin.ToString();
                selectSlotSlider.value = int.Parse(text);
            }
            else if (int.Parse(text) > itemCountMax)
            {
                selectSlotInputField.text = itemCountMax.ToString();
                selectSlotSlider.value = int.Parse(text);
            }     
        }
        else
        {
            selectSlotInputField.text = itemCountMin.ToString();
            selectSlotSlider.value = itemCountMin;
        }
    }

    void BuyStoreItem()
    {
        itemSlot.selectImage.gameObject.SetActive(false);
        selectImage2.gameObject.SetActive(true);
        BuyText();        
    }

    void SaleStoreItem()
    {
        itemSlot.selectImage.gameObject.SetActive(false);
        selectImage2.gameObject.SetActive(true);
        SaleText();        
    }

    void itemSlotActive()
    {
        itemSlot.selectImage.gameObject.SetActive(false);
        selectSlotSlider.value = itemCountMin;
    }

    // 2번째패널 "구매 및 판매" 버튼
    void YesButtonFunction()
    {
        if (isBuy)
        {
            // 구매 가능(골드가 충분 함)
            if (ui.Gold >= (int)selectSlotSlider.value * Inventory.Inst.itemData[selectIndex].buyValue)
            {
                itemSlot.Dialogue.NextDialogue(2);
                Inventory.Inst.AddItem((int)selectSlotSlider.value, (ItemEnum)selectIndex);
                ui.Gold -= (int)selectSlotSlider.value * Inventory.Inst.itemData[selectIndex].buyValue;
                selectSlotSlider.value = itemCountMin;
                selectImage2.gameObject.SetActive(false); 
            }
            else
            {
                // 골드가 부족함
                informationImage.gameObject.SetActive(true);
                NoButtonFunction();
                itemSlot.Dialogue.NextDialogue(4);
                popUpwindow.text = "골드가 부족합니다.";
            }
        }
        else
        {
            // 재료 아이템 판매 가능
            if (Inventory.Inst.inventory[selectIndex].itemCount >= (int)selectSlotSlider.value)
            {
                itemSlot.Dialogue.NextDialogue(3);
                Inventory.Inst.SubItem((int)selectSlotSlider.value, (ItemEnum)Inventory.Inst.itemData[selectIndex].itemID);
                ui.Gold += (int)selectSlotSlider.value * Inventory.Inst.itemData[selectIndex].SaleValue;
                selectSlotSlider.value = itemCountMin;
                selectImage2.gameObject.SetActive(false); 
            }
            else
            {
                // 판매할 재료가 부족함
                informationImage.gameObject.SetActive(true);
                NoButtonFunction();
                itemSlot.Dialogue.NextDialogue(5);
                popUpwindow.text = "판매할 아이템이 부족합니다.";
            }
        }

        Debug.Log($"{Inventory.Inst.inventory[selectIndex].itemCount}");
    }


    // 2번째패널 "취소" 버튼
    void NoButtonFunction()
    {
        //Inventory.Inst.SubItem((int)selectSlotSlider.value, (ItemEnum)Inventory.Inst.itemData[selectIndex].itemID);
        selectImage2.gameObject.SetActive(false);
        selectSlotSlider.value = itemCountMin;
    }

    void BuyText()
    {
        isBuy = true;

        //다이아몬드 아이템을 999개 구매하시겠습니까?
        informationText.text = $"{Inventory.Inst.itemData[selectIndex].itemName} 아이템을 {(int)selectSlotSlider.value}개 구매하시겠습니까?";
        //소지한 아이템 수 9999999
        possessionitem.text = $"소지한 아이템 수 {Inventory.Inst.inventory[selectIndex].itemCount}";
        //총 999999G 입니다.
        itemPriceText.text = $"총 {(int)selectSlotSlider.value * Inventory.Inst.itemData[selectIndex].buyValue :#,0} G 입니다.";

    }

    void SaleText()
    {
        isBuy = false;

        //다이아몬드 아이템을 999개 판매하시겠습니까?
        informationText.text = $"{Inventory.Inst.itemData[selectIndex].itemName} 아이템을 {(int)selectSlotSlider.value}개 판매하시겠습니까?";
        //소지한 아이템 수 9999999
        possessionitem.text = $"소지한 아이템 수 {Inventory.Inst.inventory[selectIndex].itemCount}";
        //총 999999G 입니다.
        itemPriceText.text = $"총 {(int)selectSlotSlider.value * Inventory.Inst.itemData[selectIndex].SaleValue :#,0} G 입니다.";
                
    }
}
