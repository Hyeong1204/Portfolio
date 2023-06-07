using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TempItemSlotUI : ItemSlotUI
{
    InventoryUI invenUI;

    /// <summary>
    /// 이 임시 슬롯이 포합된 인벤토리를 가지고 있는 플레이어
    /// </summary>
    Player owner;
    /// <summary>
    /// 임시 슬롯이 열리고 닫힘을 알리는 델리게이트 true면 열림. false면 닫힘
    /// </summary>
    public Action<bool> onTempSlotOpenClose;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        transform.position = Mouse.current.position.ReadValue();        // 매 프레임마다 마우스 위치로 이동
    }

    /// <summary>
    /// 슬롯 초기화 함수
    /// </summary>
    /// <param name="id">슬롯의 ID</param>
    /// <param name="slot">이 UI가 보여줄 ItemSlot</param>
    /// <param name="owner">이 UI의 사용하는 플레이어 ItemSlot</param>
    public override void InitializeSlot(uint id, ItemSlot slot)
    {
        onTempSlotOpenClose = null;                 // 델리게이트 초기화
        invenUI = Gamemanager.Inst.InvenUI;
        owner = invenUI.Owner;
        base.InitializeSlot(id, slot);
    }

    /// <summary>
    /// TempItemSlot를 여는 함수
    /// </summary>
    public void Open()
    {
        if (!ItemSlot.IsEmpty)                  // 아이템이 들어잇을 때만 열기
        {
            transform.position = Mouse.current.position.ReadValue();        // 열릴 때 마우스 위치로 이동
            onTempSlotOpenClose?.Invoke(true);  // 열렸다고 알림
            gameObject.SetActive(true);         // 활성화
        }
    }

    /// <summary>
    /// TempItemSlot를 닫는함수
    /// </summary>
    public void Close()
    {
        onTempSlotOpenClose?.Invoke(false);     // 닫혔다고 알림
        gameObject.SetActive(false);            // 비활성화
    }

    public void OnDrop(InputAction.CallbackContext _)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();             // 스크린 좌표 가져오기
        if (!invenUI.IsInInvantoryArea(screenPos) && !ItemSlot.IsEmpty)     // 스크린 좌표가 인벤토리에 영역 밖이고 임시 슬롯에 아이템이 있을 때
        {
            //RectTransform rectTransform = (RectTransform)transform.parent;      
            //float xPos = rectTransform.sizeDelta.x;
            //float yPos = rectTransform.sizeDelta.y;
            //if (screenPos.y < rectTransform.position.y || screenPos.y > rectTransform.position.y + yPos || screenPos.x < rectTransform.position.x - xPos || screenPos.x > rectTransform.position.x)
            //{
                Ray ray = Camera.main.ScreenPointToRay(screenPos);              // 스크린 좌표로 레이 생성
                if (Physics.Raycast(ray, out RaycastHit hit, 1000.0f, LayerMask.GetMask("Ground")))     // 레이와 땅의 충돌 여부 확인
                {
                    // 레이와 땅이 충돌했으면
                    Vector3 dropDir = hit.point - owner.transform.position;     // 피킹된 지점과 플레이어의 위치를 계산해서 방향 벡터 구하기
                    Vector3 dropPos = hit.point;            // 피킹한 지점 따로 저장
                    if (dropDir.sqrMagnitude > owner.itemPickupRange * owner.itemPickupRange)
                    {
                        // 피킹한 지점이 너무 멀리 떨어져 있으면
                        dropPos = owner.transform.position + dropDir.normalized * owner.itemPickupRange;        // 플레이어 위치에서 일정 범위를 벗어나지 않도록 처리
                    }

                // 아이템을 땅에 버릴 때 아이템을 장착하고 있으면 해제하고 버리기
                if (ItemSlot.IsEquipped)
                {
                    ItemData_EquipItem equipitem = ItemSlot.ItemData as ItemData_EquipItem;
                    if(equipitem != null)
                    {
                        equipitem.UnEquipItem(owner.gameObject, ItemSlot);
                    }
                }

                    // 아이템 생성
                    ItemFactory.MakeItems((int)ItemSlot.ItemData.id, (int)ItemSlot.ItemCount, dropPos, true);
                    ItemSlot.ClearSlotItem();       // 임시 슬롯 비우고
                    Close();                        // 임시 슬롯 닫기
                }
            //}
        }
    }
}
