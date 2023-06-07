using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlots : MonoBehaviour
{
    ItemSlot[] slots;
    ItemSlot selectSlot;

    List<int> randomItem;

    BlackMarket shop;

    private void Awake()
    {
        slots = GetComponentsInChildren<ItemSlot>();
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].onSlotClick += Selected;
        }
        shop = transform.parent.parent.GetComponent<BlackMarket>();
        randomItem = new List<int>(slots.Length);       // 무조건 slots갯수만큼만 할것이기 때문에
    }

    /// <summary>
    /// 슬롯이 클릭 될때 해당 슬롯 저장하는 함수
    /// </summary>
    void Selected(ItemSlot slot)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == slot)
            {
                selectSlot = slots[i];
                shop.SelectedSlot(slot);
                break;
            }
        }
    }

    public void RandomitemSetting()
    {
        int rand;
        int index = Enum.GetValues(typeof(ItemEnum)).Length-1;
        while (randomItem.Count < slots.Length)
        {
            rand = UnityEngine.Random.Range(0, index);

            if (!randomItem.Contains(rand))
            {
                randomItem.Add(rand);
            }
        }

        randomItem.Sort();      // 오름차순으로 정렬

        for (int i = 0; i < randomItem.Count; i++)
        {
            slots[i].SlotInitialize((ItemEnum)randomItem[i], shop.ItemSaleRatio);      // 해당 슬롯 설정
        }
    }
}
