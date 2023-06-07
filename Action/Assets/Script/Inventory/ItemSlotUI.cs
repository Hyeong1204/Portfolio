using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    // 변수  ------------------------------------------------------------------------------------------------
    private uint id;        // 몇번째 슬롯인가?

    protected ItemSlot itemSlot; // 이 UI와 연결된 ItemSlot

    
    Image itemImage;
    TextMeshProUGUI itemCountText;
    TextMeshProUGUI itemEquipText;
    // 프로퍼티 ----------------------------------------------------------------------------------------------
    public uint ID => id;

    public ItemSlot ItemSlot => itemSlot;

    // 델리게이트  --------------------------------------------------------------------------------------------
    public Action<uint> onDragStart;            // 드래그 시작했을 때
    public Action<uint> onDragEnd;              // 드래그 끝났을 때(자신 안에서 끝)
    public Action<uint> onDragCanel;            // 드래그 실패했을 떄(자신 박에서 끝)
    public Action<uint> onClick;                // 클릭이 되었을 때
    public Action<uint> onShiftClick;            // 쉬프트 클릭이 되었을 때
    public Action<uint> onPointerEnter;         // 마우스 포인터가 안에 들어왔을 때
    public Action<uint> onPointerExit;          // 마우스 포인터가 박으로 나갔을 때
    public Action<Vector2> onPointerMove;       // 마우스 포인터가 안에서 움직일 때
    // 함수 --------------------------------------------------------------------------------------------------
    protected virtual void Awake()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
        itemCountText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        itemEquipText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();        
    }

    /// <summary>
    /// 슬롯 초기화 함수
    /// </summary>
    /// <param name="id">슬롯의 ID</param>
    /// <param name="slot">이 UI가 보여줄 ItemSlot</param>
    public virtual void InitializeSlot(uint id, ItemSlot slot)
    {
        this.id = id;
        this.itemSlot = slot;
        this.itemSlot.onSlotItemChange = Refresh;
        this.ItemSlot.onSlotitemEquip = SetEquipMark;
        
        onDragStart = null;
        onDragEnd = null;
        onDragCanel = null;
        onClick = null;
        onShiftClick = null;
        onPointerEnter = null;
        onPointerExit = null;
        onPointerMove = null;
        Refresh();

        ClearEquipMark();
    }

    /// <summary>
    /// 자식 게임 오브젝트들의 크기 변경
    /// </summary>
    /// <param name="iconSize">아이콘 한변의 크기</param>
    public void Resize(float iconSize)
    {
        RectTransform rectTransform = (RectTransform)itemImage.gameObject.transform;
        rectTransform.sizeDelta = new Vector2(iconSize, iconSize);
    }

    /// <summary>
    /// 슬롯에 보이는 모습 갱신용도의 함수. itemSlot의 내부 데디어터가 변경될 때마다 실행.
    /// </summary>
    void Refresh()
    {
        if( itemSlot.IsEmpty)
        {
            // 슬롯에 아이템이 비었으면
            itemImage.sprite = null;                // 스프라이트 빼기
            itemImage.color = Color.clear;          // 투명화 (스프라이트 빼면 흰색 네모만 나옴)
            itemCountText.text = null;               // 갯수 비우기
        }
        else
        {
            itemImage.sprite = itemSlot.ItemData.itemIcon;      // 해당 아이콘 이미지 표시
            itemImage.color = Color.white;                      // 불투명화
            itemCountText.text = $"{ItemSlot.ItemCount}";       // 아이템 갯수 설정
        }
    }

    public void SetEquipMark(bool isEqip)
    {
        if (isEqip)
        {
            itemEquipText.color = Color.red;
        }
        else
        {
            itemEquipText.color = Color.clear;
        }
    }

    public void ClearEquipMark()
    {
        SetEquipMark(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // OnBeginDrag, OnEndDrag를  실해시키기 위해 추가

        //eventData.position : 마우스 포인터의 스크린 좌표값
        //eventData.delta : 마우스 포인터의 위치 변화량
        //eventData.button == PointerEventData.InputButton.Left : 마우스 왼쪽 버튼이 눌러져 있다.
        //eventData.button == PointerEventData.InputButton.Right : 마우스 오른쪽 버튼이 눌러져 있다.
    }

    /// <summary>
    /// EventSystems에서 드래그 시작을 감지하면 실행되는 함수
    /// </summary>
    /// <param name="eventData">관련 이벤트 정보들</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"드래그 시작 : {ID}번 슬롯");
        onDragStart?.Invoke(ID);        // 이 슬롯에서 드래그가 시작되었음을 알림
    }

    /// <summary>
    /// EventSystems에서 종료가 감지되면 실행된는 함수
    /// </summary>
    /// <param name="eventData">관련 이벤트 정보들</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;        // 현재 마우스 위치에 피킹된 오브젝트가 있는지 확인
        if (obj != null)
        {
            // 피킹된 것이 있다. (대부분 UI)
            ItemSlotUI endSlot = obj.GetComponent<ItemSlotUI>();                // 피킹된 오브젝트에서 ItemSlotUI가져오기
            if (endSlot != null)
            {
                onDragEnd?.Invoke(endSlot.ID);                                  // 피킹된 슬롯에서 드래그가 끝났음을 알림
            }
            else
            {
                Debug.Log($"드래그 실패 : {ID}번째 슬롯에서 실패");
                onDragCanel?.Invoke(ID);                                        // 드래그가 실패 했음을 알림
            }
        }
    }

    /// <summary>
    /// EventSystems에서 클릭이 감지되면 실행되는 함수
    /// </summary>
    /// <param name="eventData">관련 이벤트 정보들</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if(Keyboard.current.leftShiftKey.ReadValue() > 0)
        {
            if (ItemSlot.ItemCount > 1)
            {
                onShiftClick?.Invoke(ID);       // 쉬프트 클릭으로 아이템 분리
            }
        }
        else
        {
            onClick?.Invoke(ID);            // 들고 있던 임시 슬롯의 아이템 배치용도
        }
    }

    /// <summary>
    /// EventSystems에서 마우스 포인터가 이 UI 영역에 들어오면 실행되는 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter?.Invoke(ID);
    }

    /// <summary>
    /// EventSystems에서 마우스 포인터가 이 UI 영역을 나가면 실행되는 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit?.Invoke(ID);
    }

    /// <summary>
    /// EventSystems에서 마우스 포인터가 이 UI 영역안에서 움직이면 실행되는 함수
    /// </summary>
    /// <param name="eventData">관련 이벤트 정보들</param>
    public void OnPointerMove(PointerEventData eventData)
    {
        onPointerMove?.Invoke(eventData.position);          // 스크린 좌표값 넘겨주기
    }
}
