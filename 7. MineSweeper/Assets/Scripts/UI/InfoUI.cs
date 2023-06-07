using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoUI : MonoBehaviour
{
    TextMeshProUGUI actionCountText;
    TextMeshProUGUI findCointText;
    TextMeshProUGUI notFindCointText;

    private void Awake()
    {
        actionCountText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        findCointText = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        notFindCointText = transform.GetChild(5).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameManager gameManager = GameManager.Inst;
        gameManager.onActionCountChange += OnActionCountChange;         // 게임 메니저의 델리게이트에 함수 등록
        OnActionCountChange(0);                                         // 첫번째 UI 초기화

        gameManager.onGameOver += () => OnGameEnd(gameManager.Board.FoundMineCount, gameManager.Board.NotFoundMineCount);
        gameManager.onGameClear += () => OnGameEnd(gameManager.Board.FoundMineCount, gameManager.Board.NotFoundMineCount);
        gameManager.onGameReset += () =>
        {
            findCointText.text = "???";
            notFindCointText.text = "???";
        };
    }

    /// <summary>
    /// ActionCount UI에 표시된는 글자 변경하는 함수
    /// </summary>
    /// <param name="count">표시될 횟수</param>
    void OnActionCountChange(int count)
    {
        actionCountText.text = $"{count:d3} 회";
    }

    void OnGameEnd(int foundCount, int notFoundCount)
    {
        findCointText.text = $"{foundCount:d3} 개";
        notFindCointText.text = $"{notFoundCount:d3} 개";
    }
}
