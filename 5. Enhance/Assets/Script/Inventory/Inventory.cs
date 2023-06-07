using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    public ItemData[] itemData;        // 총 아이템 데이터

    public Item[] inventory;
    ItemEnum selectItem = ItemEnum.NULL;
    protected override void Initialize()
    {
        base.Initialize();
        inventory = new Item[itemData.Length];
        for (int i = 0; i < itemData.Length; i++)
        {
            GameObject obj = new GameObject();
            obj.transform.parent = transform;
            obj.name = itemData[i].name;
            obj.AddComponent<Item>();
            inventory[i] = obj.GetComponent<Item>();
            inventory[i].ItemType = (ItemEnum)i;
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// 아이템 추가
    /// </summary>
    /// <param name="count">증가 개수</param>
    /// <param name="code">아이템 코드</param>
    /// <returns></returns>
    public bool AddItem(int count, ItemEnum code)
    {
        bool result = false;

        if (IsValidItem(code) && selectItem != ItemEnum.NULL)
        {
            inventory[(int)selectItem].itemCount += count;

            result = true;
        }

        return result;
    }

    /// <summary>
    /// 아이템 감소
    /// </summary>
    /// <param name="count">감소 개수</param>
    /// <param name="code">아이템 코드</param>
    /// <returns></returns>
    public bool SubItem(int count, ItemEnum code)
    {
        bool result = false;

        if (IsValidItem(code) && selectItem != ItemEnum.NULL)
        {
            if ((inventory[(int)selectItem].itemCount - count) >= 0)
            {
                inventory[(int)selectItem].itemCount -= count;

                result = true; 
            }
        }

        return result;
    }

    /// <summary>
    /// 해당 아이템이 있는지 검사
    /// </summary>
    /// <param name="code"></param>
    /// <returns>없으면 null, 있으면 그아이템 선택</returns>
    bool IsValidItem(ItemEnum code)
    {
        bool result = false;

        if (inventory[(int)code].ItemType == code)
        {
            result = true;
            selectItem = code;
        }
        else
        {
            selectItem = ItemEnum.NULL;
        }

        return result;
    }
}
