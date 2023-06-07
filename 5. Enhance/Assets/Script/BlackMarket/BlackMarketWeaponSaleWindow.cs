using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BlackMarketWeaponSaleWindow : MonoBehaviour
{
    public Color selectColor;
    public Color ideColor;

    BlackMarket shop;
    BlackMarketWeaponPanel[] weaponPanels;
    BlackMarketWeaponPanel selectWeaponPanel = null;
    int selectIndex = -1;

    Image[] selectImages;
    Image selectBox;

    Button exitButton;
    Button saleButton;

    Dialogue dialogue;

    public System.Action<string> onWeaponSaleWindowPopUP;
    public System.Action onSaleWeapon;

    private void Awake()
    {
        shop = GetComponentInParent<BlackMarket>();
        weaponPanels = GetComponentsInChildren<BlackMarketWeaponPanel>();

        for (int i = 0; i < weaponPanels.Length; i++)
        {
            weaponPanels[i].onPanelClick += SelectPanel;
        }

        selectImages = new Image[weaponPanels.Length];
        for (int i = 0; i < selectImages.Length; i++)
        {
            selectImages[i] = weaponPanels[i].transform.GetComponent<Image>();
        }

        selectBox = transform.GetChild(6).GetComponent<Image>();

        exitButton = transform.GetChild(5).GetComponent<Button>();
        exitButton.onClick.AddListener(Close);
        saleButton = transform.GetChild(4).GetComponent<Button>();
        saleButton.onClick.AddListener(WeaponSale);
        saleButton.interactable = false;

        dialogue = FindObjectOfType<Dialogue>();
    }

    private void Start()
    {

        Close();
    }

    public void Open()
    {
        GameManager gameManager = GameManager.Inst;

        for (int i = 0; i < weaponPanels.Length; i++)
        {
            weaponPanels[i].Setting(gameManager.Weapons[i], shop.WeaponSaleRatio);
        }

        transform.gameObject.SetActive(true);
    }

    void Close()
    {
        SelectRest();
        transform.gameObject.SetActive(false);
    }

    void SelectPanel(BlackMarketWeaponPanel panel)
    {
        if (selectWeaponPanel != panel)
        {
            // 선택
            if (selectWeaponPanel == null)
            {
                saleButton.interactable = true;      // 버튼 활성화
            }

            selectWeaponPanel = panel;

            for (int i = 0; i < weaponPanels.Length; i++)
            {
                if (weaponPanels[i] == panel)
                {
                    // 선택한 슬롯이면
                    selectImages[i].color = selectColor;
                    selectBox.transform.SetParent(selectImages[i].transform);
                    selectBox.rectTransform.anchoredPosition = Vector3.zero;
                    selectIndex = i;
                }
                else
                {
                    // 선택한 슬롯이 아니면
                    selectImages[i].color = ideColor;
                }
            }
            selectBox.gameObject.SetActive(true);
        }
        else
        {
            SelectRest();
        }
    }

    void SelectRest()
    {
        selectIndex = -1;
        // 선택 해제
        selectWeaponPanel = null;
        saleButton.interactable = false;
        for (int i = 0; i < weaponPanels.Length; i++)
        {
            selectImages[i].color = ideColor;
        }
        selectBox.transform.SetParent(transform);
        selectBox.gameObject.SetActive(false);
    }

    void WeaponSale()
    {
        GameManager gameManager = GameManager.Inst;
        string text;

        if (gameManager.Weapons[selectIndex].EnganceRange != 0)
        {
            gameManager.BlackMarketSaleWeapon(selectIndex, shop.WeaponSaleRatio);
            text = "무기를 판매하여 명성치가 감소 됩니다.";
            onSaleWeapon?.Invoke();
            dialogue.NextDialogue(5);
            Close();
        }
        else
        {
            text = "판매 가능한 무기가 없습니다.";
            dialogue.NextDialogue(6);
        }

        onWeaponSaleWindowPopUP?.Invoke(text);
    }
}
