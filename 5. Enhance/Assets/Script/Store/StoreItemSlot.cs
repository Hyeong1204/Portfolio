using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreItemSlot : MonoBehaviour, IPointerClickHandler
{
    TextMeshProUGUI itemName;
    TextMeshProUGUI itemPrice;
    Dialogue dialogue;
    public Image selectImage;

    public TextMeshProUGUI ItemName => itemName;
    public TextMeshProUGUI ItemPrice => itemPrice;
    public Dialogue Dialogue => dialogue;


    public Action<StoreItemSlot> onStoreItemSlot;
    private void Awake()
    {
        itemName = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        itemPrice = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        dialogue = FindObjectOfType<Dialogue>();
        selectImage = transform.parent.GetChild(11).GetComponent<Image>();
    }
    void Start()
    {
        selectImage.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onStoreItemSlot?.Invoke(this);
        dialogue.NextDialogue(1); //다이얼로그 출력
        selectImage.gameObject.SetActive(true);

        Debug.Log($"{gameObject.name}를 선택했습니다");
    }
}
