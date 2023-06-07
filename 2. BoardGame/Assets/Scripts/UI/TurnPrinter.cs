using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnPrinter : MonoBehaviour
{
    TextMeshProUGUI turnText;

    private void Awake()
    {
        turnText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        TurnManager turnManager = TurnManager.Inst;
        turnManager.onTurnStart += OnTextRefresh;

        turnText.text = $"1턴";
    }

    /// <summary>
    /// 턴 텍스트 업데이트
    /// </summary>
    /// <param name="turnCount"></param>
    private void OnTextRefresh(int turnCount)
    {
        turnText.text = $"{turnCount}턴";
    }
}
