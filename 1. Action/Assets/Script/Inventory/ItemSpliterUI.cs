using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;

public class ItemSpliterUI : MonoBehaviour, IScrollHandler
{
    /// <summary>
    /// 분리할 갯수의 최소값
    /// </summary>
    const int itemCountMin = 1;
    int itemCountMax;

    /// <summary>
    /// 분리할 갯수
    /// </summary>
    uint itemSplitCount = itemCountMin;

    float xPos;
    float yPos;

    ItemSlot targetSlot;

    /// <summary>
    /// 분리할 갯수 입력을 위한 인풋 필드
    /// </summary>
    TMP_InputField inputField;

    /// <summary>
    /// 분리할 갯수 입력을 위한 솔라이더
    /// </summary>
    Slider slider;

    /// <summary>
    /// 분리할 아이템 아이콘
    /// </summary>
    Image itemImage;

    /// <summary>
    /// 부닐할 갯수 설정 및 확인을 위한 프로퍼티
    /// </summary>
    uint ItemSplitCount
    {
        get => itemSplitCount;
        set
        {
            if (itemSplitCount != value)        // 분리할 갯수에 변경이 있을 때 
            {
                itemSplitCount = value;
                itemSplitCount = (uint)Mathf.Max((uint)1, itemSplitCount);      // 최소 값은 1
                
                if(targetSlot != null)
                {
                    itemSplitCount = (uint)Mathf.Min(itemSplitCount, targetSlot.ItemCount - 1); // 최대 값은 슬롯에 들어있는 갯수 -1
                }
                // 결정된 분리 갯수를 인풋필드와 슬라이더로 표현
                inputField.text = itemSplitCount.ToString();
                slider.value = itemSplitCount;
            }
        }
    }

    /// <summary>
    /// OK 버튼을 눌렀을 때 실행될 델리게이트
    /// </summary>
    public Action<uint, uint> OnOkClick;

    private void Awake()
    {
        // 각종 초기화
        inputField = GetComponentInChildren<TMP_InputField>();                                       // 인풋 필드 컴포넌트 찾기
        inputField.onValueChanged.AddListener((text) => ItemSplitCount = (uint)int.Parse(text));     // 인풋 필드의 값이 변경될 때 변경된 값이 ItemSplitCount에 적용

        slider = GetComponentInChildren<Slider>();                  // 슬라이더 컴포넌트 찾기
        slider.onValueChanged.AddListener(ChangeSliderValue);       // 슬라이더의 값이 변경될 때 변경된 값이 ItemSplitCount에 적용

        Button increase = transform.GetChild(1).GetComponent<Button>();     // 증가버튼 컴포넌트 찾기
        increase.onClick.AddListener(Add);                                  // 눌러질 때 마다 ItemSplitCount 1씩 증가
        Button decrease = transform.GetChild(2).GetComponent<Button>();     // 감소버튼 컴포넌트 찾기
        decrease.onClick.AddListener(Decrease);                             // 눌러질 때 마다 ItemSplitCount 1씩 감소

        Button ok = transform.GetChild(4).GetComponent<Button>();           // ok 버튼 컴포넌트 찾기
        ok.onClick.AddListener(() => 
        {
            OnOkClick?.Invoke(targetSlot.SlotIndex, ItemSplitCount);        // 어떤 슬롯에서 몇개의 아이템을 옮길지 알려주기
            Close();                                                        // 아이템 분리창 닫기
        });      // ok 버튼이 눌러지면 InventoryUI에 알림
        Button cancel = transform.GetChild(5).GetComponent<Button>();       // 캔슬 버튼이 컴포넌트찾기
        cancel.onClick.AddListener(Close);                                  // 캔슬 버튼이 눌러지면 아이템 분리창 닫기

        itemImage = transform.GetChild(6).GetComponent<Image>();            // 아이템 아이콘을 표시할 이미지 컴포넌트찾기
    }

    private void Start()
    {
        slider.minValue = itemCountMin;     // 최소값 설정
        RectTransform rectTransform = (RectTransform)transform;
        xPos = rectTransform.sizeDelta.x * 0.5f;
        yPos = rectTransform.sizeDelta.y;

        Close();        // 시작할 때 닫고 시작하기
    }

    /// <summary>
    /// 아이템 분리창을 여는 함수
    /// </summary>
    /// <param name="target">아이템을 분리할 슬롯</param>
    public void Open(ItemSlotUI target)
    {
        if (!target.ItemSlot.IsEmpty)
        {
            targetSlot = target.ItemSlot;           // 슬롯 가져오기
            ItemSplitCount = 1;                                     // 아이템 분리갯수 초기화
            itemImage.sprite = targetSlot.ItemData.itemIcon;        // 아이콘 설정
            itemCountMax = (int)targetSlot.ItemCount - 1;           // 현재 가지고있는 갯수 -1로 설정  
            slider.maxValue = itemCountMax;
            
            inputField.text = itemSplitCount.ToString();
            this.gameObject.SetActive(true);                        // 실제로 활성화해서 보여주기
        }
    }

    /// <summary>
    /// 아이템 분리창을 닫는 함수
    /// </summary>
    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 슬라이더의 값이 변경될 때 변경된 값이 ItemSplitCount에 적용
    /// </summary>
    /// <param name="Value"></param>
    private void ChangeSliderValue(float Value)
    {
        ItemSplitCount = (uint)Mathf.RoundToInt(Value);
    }

    void Add()
    {
        ItemSplitCount++;
    }

    void Decrease()
    {
        ItemSplitCount--;
    }

    /// <summary>
    /// 마우스가 클릭되었을 때 실행될 함수
    /// </summary>
    /// <param name="context"></param>
    public void OnMouseClick(InputAction.CallbackContext context)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        if (IsAreaInside(screenPos) && gameObject.activeSelf)       // 열려있는 상태이고 마우스의 위치가 아이템 분리기 안에 있는지 밖에 있는지 확인
        {
            Close();                            // 밖에 있으면 닫기
        }
    }

    /// <summary>
    /// 마우스 휠이 움직였을 떄 실행되는 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnScroll(PointerEventData eventData)
    {
        if(eventData.scrollDelta.y > 0)
        {
            Add();          // 위쪽으로 휠을 올리면 갯수 증가
        }
        else
        {
            Decrease();     // 아래쪽으로 휠을 내리면 갯수 감소
        }
    }

    /// <summary>
    /// 특정 스키린좌표가 아이템 분리기 안에 있는지 밖에 있는지 확인하는 함수
    /// </summary>
    /// <param name="screenPos">확인할 스크린 좌표</param>
    /// <returns>true면 스크린좌표가 아이템 분리기 밖에 있다.</returns>
    private bool IsAreaInside(Vector2 screenPos)
    {
        // 결과는 위치가 스크린좌표가 transform.position위치에 벗어 나면 true
        return (screenPos.y < transform.position.y || screenPos.y > transform.position.y + yPos || screenPos.x < transform.position.x - xPos || screenPos.x > transform.position.x + xPos);
    }
}
