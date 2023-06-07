using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreWeaponSlot : MonoBehaviour, IPointerClickHandler
{
    Image weaponpanel;
    Image weaponImage;
    TextMeshProUGUI weaponName;
    TextMeshProUGUI weaponPrice;

    public Image Weaponpanel => weaponpanel;
    public Image WeaponImage => weaponImage;
    public TextMeshProUGUI WeaponName => weaponName;
    public TextMeshProUGUI WeaponPrice => weaponPrice;

    public Action<StoreWeaponSlot> onSelectWeapon;

    private void Awake()
    {
        weaponpanel = transform.GetComponent<Image>();
        weaponImage = transform.GetChild(0).GetComponent<Image>();
        weaponName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        weaponPrice = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onSelectWeapon?.Invoke(this);
        //Debug.Log($"{gameObject.name}를 선택했습니다");        
    }
}
