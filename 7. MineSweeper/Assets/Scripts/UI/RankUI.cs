using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankUI : MonoBehaviour
{
    TextMeshProUGUI rank;
    TextMeshProUGUI record;
    TextMeshProUGUI countWord;

    private void Awake()
    {
        rank = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        record = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        countWord = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 순위와 기록을 텍스트로 출력하는 함수
    /// </summary>
    /// <param name="rank">순위</param>
    /// <param name="record">기록</param>
    public void SetRankAndRecord<T>(int rankData, T recordData)
    {
        rank.text = $"{rankData}등";
        record.text = $"{recordData:N0}";
        countWord.enabled = true;
    }

    /// <summary>
    /// 순위와 기록을 텍스트로 출력하는 함수
    /// </summary>
    /// <param name="rank">순위</param>
    /// <param name="record">기록</param>
    //public void SetRankAndRecord(int rankData, float recordData)
    //{
    //    rank.text = $"{rankData}등";
    //    record.text = $"{recordData}";
    //    countWord.enabled = true;
    //}

    /// <summary>
    /// 갯수를 나타내는 말을 변경하는 함수
    /// </summary>
    /// <param name="str">갯수를 나타내는 말. ex) 개, 초, 회 등등</param>
    public void SetCountWord(string str)
    {
        countWord.text = str;
    }

    /// <summary>
    /// 이 RankLine을 안 보이게 하는 함수
    /// </summary>
    public void ClearLine()
    {
        rank.text = "";
        record.text = "";
        countWord.enabled = false;
    }
}
