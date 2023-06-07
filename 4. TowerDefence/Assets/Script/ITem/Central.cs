using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Central : MonoBehaviour
{
    public Transform invisibleItem;

    void BeginDarg(Transform Item)      // 드래그 시작
    {
        //Debug.Log("BeginDarg : " + Item.name);

        //SwapItemHierarchy(invisibleItem, Item);
    }
    void Darg(Transform Item)           // 드래그 중
    {
        //Debug.Log("Darg : " + Item.name);
    }
    void EndDarg(Transform Item)        // 드래그 끝
    {
        //Debug.Log("EndDarg : " + Item.name);

        //SwapItemHierarchy(invisibleItem, Item);
    }

    void SwapItemHierarchy(Transform sour, Transform dest)      //  스왑
    {
        Transform sourParent = sour.parent;     // sour의 부모를 가져옴
        Transform destParent = dest.parent;     // dest의 부모를 가져옴

        int sourIndex = sour.GetSiblingIndex();     // sour의 인덱스 값을 가져옴
        int destIndex = dest.GetSiblingIndex();     // dest의 인덱스 값을 가져옴

        sour.SetParent(destParent);             // sour의 부모를 dest의 부모로 바꿈
        sour.SetSiblingIndex(destIndex);        // sour의 인덱스를 dest의 인덱스 값으로 바꿈

        dest.SetParent(sourParent);             // dset의 부모를 sour의 부모로 바꿈
        dest.SetSiblingIndex(sourIndex);        // dest의 인덱스를 sour의 인덱스 값으로 바꿈
    }
}
