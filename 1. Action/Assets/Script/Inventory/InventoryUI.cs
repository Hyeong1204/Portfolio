using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    // ItemSlotUI가 있는 프리팹. 인벤토리 크키 변화에 대비해서 가지고 있기
    public GameObject slotPrefab;

    /// <summary>
    /// 이 UI가 보여줄 이벤토리
    /// </summary>
    Inventory inven;

    ItemSlotUI[] slotUIs;
    TempItemSlotUI tempSlot;

    /// <summary>
    /// 아이템 상세 정보를 보여주는 UI창
    /// </summary>
    DetaillnfoUI detail;

    ItemSpliterUI spliter;

    /// <summary>
    /// 입력 처리용 인풋 액션 클래스
    /// </summary>
    PlayerInputAction inputAction;

    /// <summary>
    /// 보유금액 표시창
    /// </summary>
    MoneyPanelUI moneyPanel;

    /// <summary>
    /// 인벤토리 창 열고 닫기 위한 캔버스 그룹
    /// </summary>
    CanvasGroup canvasGroup;

    /// <summary>
    /// 인벤토리 창 닫기용 버튼
    /// </summary>
    Button closeButton;

    public Player Owner => inven.Owner;

    public Action onInevntoryOpen; 
    public Action onInevntoryClose;

    private void Awake()
    {
        // 컴포넌트 찾기
        Transform slotParent = transform.GetChild(0);               // 가져오기 용도
        slotUIs = new ItemSlotUI[slotParent.childCount];
        for (int i = 0; i < slotParent.childCount; i++)
        {
            Transform child = slotParent.GetChild(i);
            slotUIs[i] = child.GetComponent<ItemSlotUI>();
        }
        tempSlot = GetComponentInChildren<TempItemSlotUI>();
        detail = GetComponentInChildren<DetaillnfoUI>();
        spliter = GetComponentInChildren<ItemSpliterUI>();
        spliter.OnOkClick += OnSplitOK;         // 스플리터가 가지고 있는 onOKClick 델리게이트에 함수 등록

        moneyPanel = GetComponentInChildren<MoneyPanelUI>();

        canvasGroup = GetComponent<CanvasGroup>();

        closeButton = transform.GetChild(5).GetComponent<Button>();
        closeButton.onClick.AddListener(Close);

        inputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputAction.UI.Enable();
        inputAction.UI.Click.performed += spliter.OnMouseClick;
        inputAction.UI.Click.canceled += tempSlot.OnDrop;
        inputAction.UI.InventoryOnOff.performed += InvnentoryShortCut;
    }

    private void OnDisable()
    {
        inputAction.UI.InventoryOnOff.performed -= InvnentoryShortCut;
        inputAction.UI.Click.canceled -= tempSlot.OnDrop;
        inputAction.UI.Click.performed -= spliter.OnMouseClick;
        inputAction.UI.Disable();
    }

    /// <summary>
    /// 입력받은 인벤토리에 맞게 각종 초기화 작업을하는 함수
    /// </summary>
    /// <param name="playerInven">이 UI로 표시할 인벤토리</param>
    public void InitializeInventoty(Inventory playerInven)
    {
        inven = playerInven;

        Transform slotParent = transform.GetChild(0);               // 가져오기 용도
        GridLayoutGroup grid = slotParent.GetComponent<GridLayoutGroup>();

        if ( Inventory.Default_Inventory_Size != inven.SlotCount)       // 인벤토리 쿠키가 기본과 다른 때의 처리
        {
            // 기본 사이즈와 다르면 기존 슬롯을 전부 삭제하고 새로 만들기
            foreach (var slot in slotUIs)
            {
                Destroy(slot.gameObject);       // 기본적으로 가지고 있던 슬롯 모두 삭제
            }


            // 인벤토리 크기에 따라 ItemSlotUI의 크기 변경
            RectTransform rectParent = (RectTransform)slotParent;
            float totalarea = rectParent.rect.width * rectParent.rect.height;       // slotParent의 전체 면적 계산
            float slotArea = totalarea / inven.SlotCount;                           // slot하나의 면적 구하기

            float slotSideLength = Mathf.Floor(Mathf.Sqrt(slotArea)) - grid.spacing.x;      // spacing 크기 고려해서 Slot 한변의 길이 구하기
            grid.cellSize = new Vector2(slotSideLength, slotSideLength);                    // 계산 결과 적용

            // 슬롯 새롭게 생성
            slotUIs = new ItemSlotUI[inven.SlotCount];          // 슬롯 배열을 새 크기에 맞게 새로 생성
            for (uint i = 0; i < inven.SlotCount; i++)
            {
                GameObject obj = Instantiate(slotPrefab, slotParent);       // 슬롯을 하나씩 새성
                obj.name = $"{slotParent.name}_{i}";                        // 슬롯 이름이 안겹치게 변경
                slotUIs[i] = obj.GetComponent<ItemSlotUI>();                // 슬롯을 배열에 저장
            }
        }

        // 공통 처리부분
        for (uint i = 0; i < inven.SlotCount; i++)
        {
            slotUIs[i].InitializeSlot(i, inven[i]);                     // 각 슬롯 초기화
            slotUIs[i].Resize(grid.cellSize.x * 0.75f);                 // 슬롯 크기에 맞게 내부 크기 리사이즈
            slotUIs[i].onDragStart += OnItemMoveStart;                  // 슬롯에서 드래그시작될 때 실행될 함수 연결
            slotUIs[i].onDragEnd += OnItemMoveEnd;                      // 슬롯에서 드래그가 끝날 떄 실행될 함수 연결
            slotUIs[i].onDragCanel += OnItemMoveCancel;                 // 드래그가 실패했을 때 실행될 함수 연결
            slotUIs[i].onClick += OnClick;                        // 클릭을 했을 때 실행될 함수 연결
            slotUIs[i].onShiftClick += OnItemSplit;                     // 쉬프트 클릭을 했을 때 실행될 함수 연결
            slotUIs[i].onPointerEnter += OnItemDetailOn;                // 마우스가 들어갔을 때 실행될 함수 연결
            slotUIs[i].onPointerExit += OnItemDetailOff;                // 마우스가 나갔을 때 실행될 함수 연결
            slotUIs[i].onPointerMove += OnPointerMove;                  // 마우스가 슬롯 안에서 움직일 때 실행될 함수 연결
        }

        // 임시 슬롯 초기화 처리
        tempSlot.InitializeSlot(Inventory.TempSlotIndex, inven.TempSlot);   // 임시 슬롯 초기화
        tempSlot.onTempSlotOpenClose += OnDetailPause;
        tempSlot.Close();           // 기본적으로 닫아 놓기

        // 특정 파트의 아이템이 해제되었을 때 실행될 델리게이트에 함수 연결
        Owner.onEquipItemChange += OnEquipPartClear;

        // 돈이 변경되면 moneyPanel도 갱신되게 변경
        Owner.onMoneyChange += moneyPanel.Refresh;
        moneyPanel.Refresh(Owner.Money);

        // 인벤토리 닫아 놓기
        Close();
    }

    /// <summary>
    /// 인벤토리 열기
    /// </summary>
    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        onInevntoryOpen?.Invoke();

    }

    /// <summary>
    /// 인벤토리 닫기
    /// </summary>
    public void Close()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0;
        onInevntoryClose?.Invoke();
    }

    private void InvnentoryShortCut(InputAction.CallbackContext _)
    {
        if (canvasGroup.interactable)       // 인벤토리가 열려있는지 닫혀있는지 확인
        {
            Close();        // 열려있으면 닫기
        }
        else
        {
            Open();         // 닫혀있으면 열기
        }
    }

    /// <summary>
    /// 특정한 파트에 장착되는 아이템은 모두 장착해제로 표시
    /// </summary>
    /// <param name="part">장착 해제 표시할 파트</param>
    private void OnEquipPartClear(EquipPartType part)
    {
        foreach (var slotUI in slotUIs)     // 모든 슬롯을 돌면서
        {
            ItemData_EquipItem equipItem = slotUI.ItemSlot.ItemData as ItemData_EquipItem;
            if (equipItem != null && equipItem.EquipPart == part)       // 같은 종류의 파츠면
            {
                slotUI.ClearEquipMark();        // 장착마크 해제
            }
        }
    }


    /// <summary>
    /// 확인할 스크린 좌표가 인벤토리 영역 안인지 확인하는 함수
    /// </summary>
    /// <param name="screenPos">확인할 스크린 좌표</param>
    /// <returns>이벤토리 영역 안에 잇으면 true, 아니면 false</returns>
    public bool IsInInvantoryArea(Vector2 screenPos)
    {
        RectTransform rectTransform = (RectTransform)transform;

        Vector2 min = new(rectTransform.position.x - rectTransform.sizeDelta.x, rectTransform.position.y);
        Vector2 max = new(rectTransform.position.x, rectTransform.position.y + rectTransform.sizeDelta.y);

        return (min.x < screenPos.x && screenPos.x < max.x && min.y < screenPos.y && screenPos.y < max.y);      // min, max 사이에 있는지 확인
    }

    /// <summary>
    /// 슬롯에 드래그를 시작햇을 때 실행될 함수
    /// </summary>
    /// <param name="slotID">드래그가 시작된 슬롯의 ID</param>
    private void OnItemMoveStart(uint slotID)
    {
        inven.MoveItem(slotID, Inventory.TempSlotIndex);    // 슬롯에 있는 아이템들을 임시 슬롯으로 모두 옮김
        tempSlot.Open();                                    // 임시 슬롯을 보여주기
    }

    /// <summary>
    /// 드래그가 슬롯에서 끝날을 때나 실패 했을 때, 클릭이 되었을 때 실행될 함수
    /// </summary>
    /// <param name="slotID">드래그가 끝난 슬롯의 ID</param>
    private void OnItemMoveEnd(uint slotID)
    {
        OnItemMoveCancel(slotID);
        detail.Open(inven[slotID].ItemData);
    }
    
    /// <summary>
    /// 마우스가 슬롯에서 클릭이 되었을 떄 실행될 함수
    /// </summary>
    /// <param name="slotID">클릭된 슬롯의 ID</param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnClick(uint slotID)
    {
        if (tempSlot.ItemSlot.IsEmpty)
        {
            // 아이템을 사용할 용도
            ItemSlot useItemSlot = inven[slotID];
            useItemSlot.UseSlotItem(Owner.gameObject);
        }
        else
        {
            // 임시 슬롯의 아이템을 slotID 슬롯에 넣을 용도
            OnItemMoveEnd(slotID);
        }
    }
    
    void OnItemMoveCancel(uint slotID)
    {
        inven.MoveItem(Inventory.TempSlotIndex, slotID);    // 임시 슬롯의 아이템들을 슬롯에 모두 옮김
        if (tempSlot.ItemSlot.IsEmpty)
        {
            tempSlot.Close();                                   // 임시 슬롯을 안바뀌게 만들기
        }
    }

    /// <summary>
    /// 마우스가 슬롯에 들어갔을 때 해당 슬롯에 있는 아이템을 상세 정보 창에서 볼 수 있도록 성정하고 여는 함수
    /// </summary>
    /// <param name="slotID">대상 슬롯</param>
    private void OnItemDetailOn(uint slotID)
    {
        detail.Open(slotUIs[slotID].ItemSlot.ItemData);         // 대상 슬롯의 아이템 데이터 넘겨주며 열기
    }

    /// <summary>
    /// 마우스가 슬롯을 나갔을 때 상세정보 창을 닫는 함수
    /// </summary>
    /// <param name="slotID"></param>
    private void OnItemDetailOff(uint _)
    {
        detail.Close();
    }

    /// <summary>
    /// 마우스가 슬롯안에서 움직일 때 실행될 함수
    /// </summary>
    /// <param name="pointerPos">마우스 포인터 스크린 좌표</param>
    private void OnPointerMove(Vector2 pointerPos)
    {
        if (detail.IsOpen)      // 디테일 창이 열려있을 때만
        {
            detail.MovePosistion(pointerPos);
        }
    }

    /// <summary>
    /// TempItemSlotUI가 열리고 닫힐 때 실행되는 함수
    /// </summary>
    /// <param name="isPause">true면 열려서 실행, false면 닫혀서 실행</param>
    private void OnDetailPause(bool isPause)
    {
        detail.IsPause = isPause;       // 임시 슬롯이 열리면 상세정보 창 일시 정지
                                        // 임시 슬롯이 닫히면 상세정보 창 일시 해제
    }

    /// <summary>
    /// 슬롯을 쉬프트 클릭했을 때 실행될 함수
    /// </summary>
    /// <param name="slotID"></param>
    private void OnItemSplit(uint slotID)
    {
        ItemSlotUI targetSlot = slotUIs[slotID];
        spliter.transform.position = targetSlot.transform.position + Vector3.up * 100;   // 스플릿터 창의 포지션을 targetSlot포지션의 Vector3.up * 100위치로 변경
        spliter.Open(targetSlot);                       // 스플릿터 창 열기
        detail.Close();                                 // 디테일 창 닫기
        detail.IsPause = true;                          // 디테일 퍼즈 온
    }

    /// <summary>
    /// 아이템 분리창에서 OK가 클릭되었을 때 실행되는 함수
    /// </summary>
    /// <param name="slotID">아이템을 분리할 슬롯</param>
    /// <param name="count">분리할 아이템 갯수</param>
    private void OnSplitOK(uint slotID, uint count)
    {
        inven.MoveItemToTempSlot(slotID, count);        // slotID 번째 슬롯에서 아이템을 count만큼 분리해서 임시슬롯에 담기
        tempSlot.Open();                                // 임시슬롯을 보이게 만들기
    }
}
