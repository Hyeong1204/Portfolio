using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    /// <summary>
    /// 모든 아이템 데이터
    /// </summary>
    public ItemData[] itemData;

    /// <summary>
    /// 배열처럼 사용할 수 있는 프로퍼티의 변종
    /// </summary>
    /// <param name="id">itemDatas의 인덱스로 사용할 변수</param>
    /// <returns>itemDatas의 id번째 아이템 데이터</returns>
    public ItemData this[uint id] => itemData[id];

    /// <summary>
    /// itemDatas 확인용 인덱서
    /// </summary>
    /// <param name="code">확인할 아이템의 code</param>
    /// <returns></returns>
    public ItemData this[ItemIDCode code] => itemData[(int)code];
}
