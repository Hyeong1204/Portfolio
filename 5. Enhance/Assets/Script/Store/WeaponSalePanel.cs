using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSalePanel : MonoBehaviour
{
    public Color selectColor;
    public Color basicColor;

    Image weaponSalePanel;
    Button exitButton;
    Button weaponSaleButton;
    Image weaponSelectBox;

    StoreWeaponSlot[] storeWeaponSlot;
    StoreWeaponSlot WeaponSlot;

    Image completePanel;
    TextMeshProUGUI completeText;
    Button completeButton;

    int selectSlot;

    private void Awake()
    {
        storeWeaponSlot = GetComponentsInChildren<StoreWeaponSlot>();

        weaponSalePanel = transform.GetComponent<Image>();

        Transform select = transform.GetChild(0).GetChild(0);
        weaponSaleButton = select.GetChild(4).GetComponent<Button>();
        weaponSaleButton.onClick.AddListener(() =>
        {
            WeaponSaleButton();            
        });

        exitButton = select.GetChild(5).GetComponent<Button>();
        exitButton.onClick.AddListener(() =>
        {            
            WeaponSalePanelOnOff(false);
            SelectReset();        
        });

        weaponSelectBox = select.GetChild(6).GetComponent<Image>();
        weaponSelectBox.gameObject.SetActive(false);

        select = transform.GetChild(0).GetChild(1);
        completePanel = select.GetComponent<Image>();
        completeText = select.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        completeButton = select.GetChild(0).GetChild(1).GetComponent<Button>();
        completeButton.onClick.AddListener(WeaponSalecompleteButton);
    }

    private void Start()
    {
        WeaponSalePanelOnOff(false);
    }

    public void WeaponSalePanelOnOff(bool flag)
    {
        completePanel.gameObject.SetActive(false);

        if (flag)
        {
            for (int i = 0; i < storeWeaponSlot.Length; i++)
            {
                storeWeaponSlot[i].WeaponImage.sprite = GameManager.Inst.Weapons[i].WeaponModel.itemIcon;
                storeWeaponSlot[i].WeaponName.text = GameManager.Inst.Weapons[i].WeaponModel.itemName;
                storeWeaponSlot[i].WeaponPrice.text = GameManager.Inst.Weapons[i].WeaponModel.saleValue.ToString("#,0") + "G";

                storeWeaponSlot[i].onSelectWeapon = null;
                storeWeaponSlot[i].onSelectWeapon += OnSelectWeapon;

                storeWeaponSlot[i].Weaponpanel.color = basicColor;
            }
        }
        weaponSaleButton.interactable = false;
        
        weaponSalePanel.gameObject.SetActive(flag);        
    }

    void OnSelectWeapon(StoreWeaponSlot slot)
    {
        if (WeaponSlot != slot)
        {
            if (WeaponSlot == null)
            {
                // 버튼 활성화
                weaponSaleButton.interactable = true;
            }

            WeaponSlot = slot;
            
            for (int i = 0; i < storeWeaponSlot.Length; i++)
            {
                if (storeWeaponSlot[i] == slot)
                {
                    // 선택한 슬롯
                    selectSlot = i;
                    storeWeaponSlot[i].Weaponpanel.color = selectColor;
                    weaponSelectBox.transform.SetParent(storeWeaponSlot[i].transform);
                    weaponSelectBox.rectTransform.anchoredPosition = Vector3.zero;
                }
                else
                {
                    // 선택하지 않은 슬롯
                    storeWeaponSlot[i].Weaponpanel.color = basicColor;
                }
            }
            weaponSelectBox.gameObject.SetActive(true);
        }
        else
        {
            SelectReset();
        }
    }

    void SelectReset()
    {
        WeaponSlot = null;
        weaponSaleButton.interactable = false;

        for (int i = 0; i < storeWeaponSlot.Length; i++)
        {
            storeWeaponSlot[i].Weaponpanel.color = basicColor;
        }

        weaponSelectBox.transform.SetParent(transform);
        weaponSelectBox.gameObject.SetActive(false);
    }

    void WeaponSaleButton()
    {
        Debug.Log("무기 판매");
        if (GameManager.Inst.Weapons[selectSlot].EnganceRange != 0) // 판매 가능
        {
            GameManager.Inst.SaleWeapon(selectSlot);
            WeaponSaleSuccess();
            storeWeaponSlot[selectSlot].WeaponImage.sprite = GameManager.Inst.WeaponManager.weaponItems[0].WeaponDatas[0].itemIcon;
            storeWeaponSlot[selectSlot].WeaponName.text = GameManager.Inst.WeaponManager.weaponItems[0].WeaponDatas[0].itemName;
            storeWeaponSlot[selectSlot].WeaponPrice.text = GameManager.Inst.WeaponManager.weaponItems[0].WeaponDatas[0].saleValue.ToString("#,0") + " G";
        }
        else
        {
            WeaponSaleFail();
        }
    }

    void WeaponSaleSuccess()
    {
        completeText.text = "무기가 판매되었습니다.";
        completePanel.gameObject.SetActive(true);
    }

    void WeaponSaleFail()
    {
        completeText.text = "판매 가능한 무기가 없습니다.";
        completePanel.gameObject.SetActive(true);
    }

    void WeaponSalecompleteButton()
    {
        completePanel.gameObject.SetActive(false);        
    }
}
