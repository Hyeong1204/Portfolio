using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class BattleLogger : MonoBehaviour
{
    TextMeshProUGUI logText;
    StringBuilder builder;
    List<string> logLines;

    const int maxLineCont = 20;
    const string YOU = "당신";
    const string ENEMY = "적";

    public Color userColor;
    public Color enemyColor;
    public Color shipColor;
    public Color turnColor;

    private void Awake()
    {
        logLines = new List<string>(maxLineCont + 5);           // 여유롭게 5더해서 잡기
        builder = new StringBuilder();                          // StringBuilder 생성
        logText = GetComponentInChildren<TextMeshProUGUI>();    // 자식에서 텍스트 찾기
    }

    private void Start()
    {
        // 턴 시작할 때 번호 출력하기 위해 델리게이트 함수 등록
        TurnManager turnManager = TurnManager.Inst;
        turnManager.onTurnStart += Log_Turn_Start;

        // 배가 피격당하고 침몰할 떄 상황을 출력하는 델리게이트 함수 등록
        GameManager gameManager = GameManager.Inst;
        foreach(var ship in gameManager.UserPlayer.Ships)
        {
            ship.onHit += (targetShip) => Log_Attack_Success(false, targetShip);
            ship.onSinking = (targetShip) => { Log_Ship_Destroy(false, targetShip); } + ship.onSinking;
        }

        foreach(var ship in gameManager.EnemyPlayer.Ships)
        {
            ship.onHit += (targetShip) => Log_Attack_Success(true, targetShip);
            ship.onSinking = (targetShip) => { Log_Ship_Destroy(true, targetShip); } + ship.onSinking;
        }

        // 플레이어가 공격을 실패했을 때 상황을 출력하기 위해서 델리게이트에 함수 등록
        gameManager.UserPlayer.onAttackFail += Log_Attack_Fail;
        gameManager.EnemyPlayer.onAttackFail += Log_Attack_Fail;

        // 플레이어의 모든 배가 파괴 되었을 때의 상황을 출력하기 위해서 델리게이트에 함수 등록
        gameManager.UserPlayer.onDefeat += Log_Defeat;
        gameManager.EnemyPlayer.onDefeat += Log_Defeat;

        Clear();        // 시작할 때 로거 비우기
    }

    void Clear()
    {
        logLines.Clear();       // 리스트 초기화
        logText.text = "";      // logText 비우기
    }

    /// <summary>
    /// 로거에 입력받은  글자를 출력하는 함수
    /// </summary>
    /// <param name="text">출력할 문자</param>
    public void Log(string text)
    {
        logLines.Add(text);                     // 리스트에 추가
        if (logLines.Count > maxLineCont)        // logLines 개수가 maxLineCont 보다 크면
        {
            logLines.RemoveAt(0);               // 첫번째 비우기
        }

        builder.Clear();                        // 기존에 있던 builder 초기화
        foreach (var line in logLines)
        {
            builder.AppendLine(line);           // builder에 하나씩 추가
        }

        logText.text = builder.ToString();      // 글자 출력하기
    }

    /// <summary>
    /// 공격이 성공했을 때 상황을 출려하는 함수
    /// </summary>
    /// <param name="isPlayerAttack">true면 유저 false면 cpuv플레이어</param>
    /// <param name="ship">공격을 당한 배</param>
    void Log_Attack_Success(bool isPlayerAttack, Ship ship)
    {
        string attackerColor;       // 공격자 색깔
        string attackerName;        // 공격자 이름

        if (isPlayerAttack)
        {
            attackerColor = ColorUtility.ToHtmlStringRGB(userColor);
            attackerName = YOU;
        }
        else
        {
            attackerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            attackerName = ENEMY;
        }

        string shipColro = ColorUtility.ToHtmlStringRGB(this.shipColor);
        string playerColor;     // 배의 소유주의 따라 색 변경
        string playerName;      // 배의 소유주의 따라 이름 변경

        if (ship.Owner is UserPlayer)       // 배의 소유주가 PlayerBase 인지 아닌지 확인
        {
            playerColor = ColorUtility.ToHtmlStringRGB(this.userColor);
            playerName = YOU;
        }
        else
        {
            playerColor = ColorUtility.ToHtmlStringRGB(this.enemyColor);
            playerName = ENEMY;
        }

        Log($"<#{attackerColor}>{attackerName}의 공격</color>\t: <#{playerColor}>{playerName}</color>의 <#{shipColro}>{ship.ShipName}</color>에 포탄이 명중 했습니다.");
    }

    void Log_Attack_Fail(PlayerBase attacker)
    {
        //"{당신}의 포탄이 빗나갔습니다."
        //"{적}의 포탄이 빗나갔습니다."

        string playerColor;
        string playerName;

        if(attacker is UserPlayer)
        {
            playerColor = ColorUtility.ToHtmlStringRGB(userColor);
            playerName = YOU;
        }
        else
        {
            playerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            playerName = ENEMY;
        }

        Log($"<#{playerColor}>{playerName}의 공격</color>\t: <#{playerColor}>{playerName}</color>의 포탄이 빗나갔습니다.");
    }

    /// <summary>
    /// 배가 침몰 당하는 상황을 출력하는 함수
    /// </summary>
    /// <param name="isPlayerAttack">true면 플레이어 공격, false면 적의 공격</param>
    /// <param name="ship">침몰한 배</param>
    void Log_Ship_Destroy(bool isPlayerAttack, Ship ship)
    {
        //"Enemy의 {배종류}를 침몰 시켰습니다."
        //"당신의 {배종류}가 침몰 했습니다."

        string attackerColor;   // 공격자 색상
        string attackerName;    // 공격자 이름
        if (isPlayerAttack)
        {
            attackerColor = ColorUtility.ToHtmlStringRGB(userColor);
            attackerName = YOU;
        }
        else
        {
            attackerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            attackerName = ENEMY;
        }

        string hexColor = ColorUtility.ToHtmlStringRGB(shipColor);  // shipColor를 16진수 표시양식으로 변경
        string playerColor; //배의 소유자가 UserPlayer면 userColor, EnemyPlayer면 enemyColor로 출력하기
        string playerName;
        if (ship.Owner is UserPlayer) // Owner이 UserPlayer 인지 아닌지 확인
        {
            playerColor = ColorUtility.ToHtmlStringRGB(userColor);
            playerName = YOU;
        }
        else
        {
            playerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            playerName = ENEMY;
        }

        Log($"<#{attackerColor}>{attackerName}의 공격</color>\t: <#{playerColor}>{playerName}</color>의 <#{hexColor}>{ship.ShipName}</color>이 침몰했습니다.");
    }

    /// <summary>
    /// 턴을 시작할 때 상황을 출력하는 함수
    /// </summary>
    /// <param name="number">현재 턴 수</param>
    void Log_Turn_Start(int number)
    {
        //"{턴숫자} 번째 턴이 시작했습니다."
        string color = ColorUtility.ToHtmlStringRGB(turnColor);
        Log($"<#{color}>{number}</color> 번째 턴이 시작했습니다.");
    }

    /// <summary>
    /// 게임이 끝날 때 상황을 출력하는 함수
    /// </summary>
    /// <param name="player">패배한 플레이어</param>
    void Log_Defeat(PlayerBase player)
    {
        if (player is UserPlayer)
        {
            // 사람이 졌다.
            Log($"당신의 <#ff0000>패배</color>입니다.");
        }
        else
        {
            // 컴퓨터가 졌다.
            Log($"당신의 <#00ff00>승리</color>입니다.");
        }
    }
}
