using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public Action<Slot> selectWeaponSlot;
    public Weapon weapon;

    private void Awake()
    {
        weapon = GetComponentInChildren<Weapon>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selectWeaponSlot?.Invoke(this);
        Debug.Log($"{gameObject.name}를 선택했습니다");
    }
}
