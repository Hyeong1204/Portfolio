using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlackMarketWeaponPanel : MonoBehaviour, IPointerClickHandler
{
    Image icon;
    TextMeshProUGUI weaponName;
    TextMeshProUGUI weaponSaleGold;

    public Action<BlackMarketWeaponPanel> onPanelClick;

    private void Awake()
    {
        icon = transform.GetChild(0).GetComponent<Image>();
        weaponName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        weaponSaleGold = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 암시장 무기판매 세팅
    /// </summary>
    public void Setting(Weapon weapon, float saleRatio)
    {
        icon.sprite = weapon.WeaponModel.itemIcon;
        weaponName.text = weapon.WeaponModel.itemName;
        weaponSaleGold.text = $"{Mathf.FloorToInt(weapon.WeaponModel.saleValue * saleRatio):#,0} g";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onPanelClick?.Invoke(this);
    }
}
