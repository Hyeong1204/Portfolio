using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인벤토리 한칸의 정보를 나타내는 클래스
public class ItemSlot
{
    uint slotIndex;                   // 슬롯의 인덱스(인벤토리의 몇 번째 슬로인가?)
    uint itemCount = 0;               // 슬롯의 들어있는 아이템 개수
    ItemData slotItemData = null;     // 슬롯의 들어있는 아이템 (null이면 아이템이 없다)

    /// <summary>
    /// 이 슬롯의 인덱스
    /// </summary>
    public uint SlotIndex => slotIndex;

    /// <summary>
    /// 이 슬롯이 비어있는지 여부(ture면 비었고, false 무엇인가 들어있다.)
    /// </summary>
    public bool IsEmpty => slotItemData == null;

    /// <summary>
    /// 이 슬롯의 아이템을 장비했는지 표시하는 변수
    /// </summary>
    bool isEquipped = false;

    public bool IsEquipped
    {
        get => isEquipped;
        set
        {
            isEquipped= value;
            onSlotitemEquip?.Invoke(isEquipped);
        }
    }

    /// <summary>
    /// 이 슬롯에 들어있는 아이템 데이터
    /// </summary>
    public ItemData ItemData
    {
        get => slotItemData;                    // 읽기는 누구나 가능
        private set                             // 쓰기는 자신만 가능
        {
            if(slotItemData != value)           // 슬롯에 아이템이 변경이 있었을 때만
            {
                slotItemData = value;
                onSlotItemChange?.Invoke();     // 델리게이트에 연결된 함수들 실행 (주로 UI 갱신용)
            }
        }
    }

    /// <summary>
    /// 아 슬롯에 들어있는 아이템 갯수
    /// </summary>
    public uint ItemCount
    {
        get => itemCount;               // 읽기는 누구나 가능
        private set                     // 쓰기는 자신만 가능
        {
            if(itemCount != value)                  // 아이템 갯수에 변경이 일어났을 때만
            {
                itemCount = value;
                onSlotItemChange?.Invoke();         // 델리게이트에 연결된 함수들 실행 (주로 UI 갱신용)
            }
        }
    }

    public ItemSlot(uint index)
    {
        slotIndex = index;
    }

    // 델리게이트 -------------------------------------------------------------------------------------
    /// <summary>
    /// 슬롯의 아이템이 변경되면 실행되는 델리게이트
    /// </summary>
    public Action onSlotItemChange;

    /// <summary>
    /// 아이템 장비되었을 때 실행되는 델리게이트
    /// </summary>
    public Action<bool> onSlotitemEquip;

    /// <summary>
    /// 이 슬롯에 지정된 아이템을 지정된 갯수로 넣는 함수
    /// </summary>
    /// <param name="data">추가할 아이템</param>
    /// <param name="count">설정된 갯수</param>
    public void AssignSlotItem(ItemData data, bool isEquip, uint count = 1)
    {
        if (data != null)       // data가 null이 아니면 파라메터대로 설정
        {
            ItemCount = count;
            IsEquipped = isEquip;
            ItemData = data;
            Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템 {ItemCount}개 설정");
        }
        else
        {
            // data가 null이면 비우는 함수 수행
            ClearSlotItem();
        }
    }

    /// <summary>
    /// 이 슬롯에서 아이템을 제거하는 함수
    /// </summary>
    public void ClearSlotItem()
    {
        ItemData = null;
        IsEquipped = false;
        ItemCount = 0;
        Debug.Log($"인벤토리 {slotIndex}번 슬롯을 비웁니다.");
    }

    /// <summary>
    /// 이 슬롯에 아이템 갯수를 증가시키는 함수
    /// </summary>
    /// <param name="overCount">넘친 갯수</param>
    /// <param name="count">증가 시킬 아이템 갯수</param>
    /// <returns>증가 성공 여부, 다 넣는 것에 성공하면 true, 넘치면 false</returns>
    public bool IncreaseSlotItem(out uint overCount, uint count = 1)
    {
        bool result;
        int over = 0;  // 아이템을 추가하려고 하는데 넘친 갯수
        
        //overCount = (int)ItemData.maxStackCount < ItemCount? (int)ItemCount - (int)ItemData.maxStackCount : 0;
        over = (int)(ItemCount + count) - (int)ItemData.maxStackCount;     // 넘친 갯수 계산
        if (over > 0)
        {
            // 아이템 최대 갯수를 넘쳤다.
            ItemCount = ItemData.maxStackCount;     // 최대치까지만 적용
            overCount = (uint)over;                 // 넘친 갯수 저장
            result = false;
        }
        else
        {
            // 충분히 추가할 수 있다.
            ItemCount += count;     // 아이템 갯수 변경
            overCount = 0;          // uderflow 방지용. 넘친 갯수 0으로 설정
            result = true;
            Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템 {count}개 만큼 증가. 현재 {ItemCount}개");
        }


        return result;
    }

    /// <summary>
    /// 이 슬롯에 아이템 갯수를 감소 시키는 함수
    /// </summary>
    /// <param name="count">감소시킬 아이템 갯수</param>
    public void DeCreaseSlotItem(uint count = 1)
    {
        int newCount = (int)ItemCount - (int)count;

        if (newCount < 1)
        {
            ClearSlotItem();
            Debug.Log($"인벤토리 {slotIndex}번 슬롯을 비웁니다.");
        }
        else
        {
            ItemCount = (uint)newCount;
            Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템 {count}개 만큼 감소. 현재 {ItemCount}개");
        }
    }

    /// <summary>
    /// 이 슬롯에 있는 아이템을 사용하는 함수
    /// </summary>
    /// <param name="target">아이템의 효과를 받을 타겟</param>
    public void UseSlotItem(GameObject target = null)
    {
        IEquipItem equip = ItemData as IEquipItem;

        if (equip != null)
        {
            // 아이템 장비처리
           equip.AutoEquipItem(target, this);
        }
        else
        {
            // 장비 아이템이 아니다.
            IUsable usable = ItemData as IUsable;       // 사용 가능한 아이템인지 확인
            if (usable != null)
            {
                if (usable.Use(target))                 // 아이템을 사용하고 성공적으로 사용했는지 확인
                {
                    DeCreaseSlotItem();                 // 아이템 갯수 1개 감소
                }
            }
        }
    }
}