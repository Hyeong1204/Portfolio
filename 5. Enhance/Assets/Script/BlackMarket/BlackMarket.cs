using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlackMarket : MonoBehaviour
{
    ItemSlot selectSlot = null;
    ItemSlots itemSlots;
    SelectWindow selectWindow;
    BlackMarketWeaponSaleWindow weaponSaleWindow;
    Button saleButton;

    // 파업창 ----------------------------------------------------
    PopUpWindow popUpWindow;
    TextMeshProUGUI popUpText;
    Button popUpButton;
    // ----------------------------------------------------------


    float itemSaleRatio;
    float weaponSaleRatio;
    bool isSaleWeapon = false;

    public float ItemSaleRatio => itemSaleRatio;
    public float WeaponSaleRatio => Mathf.Floor( weaponSaleRatio * 100.0f) / 100.0f;

    private void Awake()
    {
        selectWindow = GetComponentInChildren<SelectWindow>();
        selectWindow.purchasefail = PopUpWindowOpen;
        itemSlots = GetComponentInChildren<ItemSlots>();

        // 파업 창 -----------------------------------------------------------------
        popUpWindow = GetComponentInChildren<PopUpWindow>();
        popUpText = popUpWindow.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        popUpButton = popUpWindow.transform.GetChild(3).GetComponent<Button>();
        popUpButton.onClick.AddListener(PopUpWindowClose);
        // -------------------------------------------------------------------------

        weaponSaleWindow = GetComponentInChildren<BlackMarketWeaponSaleWindow>();
        weaponSaleWindow.onWeaponSaleWindowPopUP = PopUpWindowOpen;
        weaponSaleWindow.onSaleWeapon = () =>
        {
            isSaleWeapon = true;
        };
        saleButton = transform.GetChild(1).GetComponent<Button>();
        saleButton.onClick.AddListener(() =>
        {
            if (!isSaleWeapon)
            {
                weaponSaleWindow.Open(); 
            }
            else
            {
                PopUpWindowOpen("더 이상 무기를 판매할 수 없습니다.");
            }
        });
    }

    private void Start()
    {
        itemSaleRatio = Random.Range(0.6f, 0.7f);           // 30 ~ 40% 세일 가격
        weaponSaleRatio = Random.Range(1.5f, 2.5f);         // 150% ~ 250% 가격
        isSaleWeapon = false;
        itemSlots.RandomitemSetting();

        PopUpWindowClose();
    }

    /// <summary>
    /// 선택 슬롯
    /// </summary>
    /// <param name="slot"></param>
    public void SelectedSlot(ItemSlot slot)
    {
        selectSlot = slot;

        selectWindow.Open(selectSlot);
    }


    /// <summary>
    /// 팝업창 열기
    /// </summary>
    /// <param name="text"></param>
    void PopUpWindowOpen(string text)
    {
        popUpText.text = $"{text}";

        popUpWindow.gameObject.SetActive(true);
    }

    /// <summary>
    /// 팝업창 닫기
    /// </summary>
    void PopUpWindowClose()
    {
        popUpWindow.gameObject.SetActive(false);
    }
}
