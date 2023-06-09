using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    /// <summary>
    /// 현재 턴 번호
    /// </summary>
    int turnNumber = 0;

    /// <summary>
    /// 현재 턴이 종료 되어있는지 여부 (true면 종료. false면 아직 진행중)
    /// </summary>
    bool isTurnEnd = true;

    /// <summary>
    /// 타임 아웃으로 턴이 종료될 때 까지 남은 시간
    /// </summary>
    float turnRemainTime = 0.0f;

    /// <summary>
    /// 한 턴이 타임 아웃되는데 걸리는 시간
    /// </summary>
    public const float turnDurationTime = 10.0f;

    /// <summary>
    /// 유저 플레이어
    /// </summary>
    PlayerBase userPlayer;

    /// <summary>
    /// CPU 플레이어
    /// </summary>
    PlayerBase enemyPlayer;

    /// <summary>
    /// 턴이 시작될 때 실행될 델리게이트. 파라메터는 현재 턴 번호
    /// </summary>
    public Action<int> onTurnStart;

    /// <summary>
    /// 턴이 종료될 때 실행될 델리게이트
    /// </summary>
    Action onTurnEnd;

    // 두 플레이어가 모두 행동을 완료하면 턴이 종료.

    private void Update()
    {
        if (enemyPlayer != null)
        {
            // 턴 타임아웃 체킹
            turnRemainTime -= Time.deltaTime;
            if (turnRemainTime < 0)
            {
                OnTurnEnd();        // 타임 아웃이 되면 턴 종료
            }

            // 현재 턴이 종료되었는지 확인
            if (isTurnEnd)
            {
                OnTurnStart();      // 종료 되었으면 다음 턴 시작
            } 
        }
    }

    /// <summary>
    /// 초기화용 함수. 씬 로드가 완료된 이후에 실행 (Awake와 Start 사이에서 실행됨)
    /// </summary>
    protected override void ManagerDataReset()
    {
        base.ManagerDataReset();

        // 변수 초기화
        turnNumber = 0;
        isTurnEnd = true;
        userPlayer = FindObjectOfType<UserPlayer>();
        enemyPlayer = FindObjectOfType<EnemyPlayer>(); ;

        // 턴 시작 / 종료 델리게이트에 프렐이어들의 턴 시작 / 종료 함수들 연결
        onTurnStart = null;
        onTurnEnd = null;
        onTurnStart += userPlayer.OnPlayerTurnStart;
        onTurnEnd += userPlayer.OnPlayerTurnEnd;

        if (enemyPlayer != null)
        {
            onTurnStart += enemyPlayer.OnPlayerTurnStart;
            onTurnEnd += enemyPlayer.OnPlayerTurnEnd;

            // 유저 플레이어의 행동이 완료 되면 적 플레이어의 행동 완료 여부를 체크해서 적의 행동도 완료 되었으면 턴 종료 실행
            userPlayer.onActionEnd += () =>
            {
                if (enemyPlayer.IsActionDone && !userPlayer.IsDepeat)    // 추가로 유저가 살아있을 때만 턴 종료
                {
                    OnTurnEnd();
                }
            };

            // 적 플레이어의 행동이 완료 되면 적 플레이어의 행동 완료 여부를 체크해서 유저의 행동도 완료 되었으면 턴 종료 실행
            enemyPlayer.onActionEnd += () =>
            {
                if (userPlayer.IsActionDone && !enemyPlayer.IsDepeat)   // 추가로 적이 살아있을 때만 턴 종료
                {
                    OnTurnEnd();
                }
            };
        }
    }

    /// <summary>
    /// 턴이 시작될 때 실행되는 함수
    /// </summary>
    void OnTurnStart()
    {
        Debug.Log("턴 시작");
        isTurnEnd = false;
        turnRemainTime = turnDurationTime;

        onTurnStart?.Invoke(turnNumber);
    }

    /// <summary>
    /// 턴이 종료될 때 실행되는 함수
    /// </summary>
    void OnTurnEnd()
    {
        //
        if (!(userPlayer.IsDepeat || enemyPlayer.IsDepeat))
        {
            Debug.Log("턴 종료");
            onTurnEnd?.Invoke();

            isTurnEnd = true;
            turnNumber++;
        }
        else
        {
            turnRemainTime = float.MaxValue;        // OnTurnEnd가 과하게 호출되는 것 방지
        }
    }
}
