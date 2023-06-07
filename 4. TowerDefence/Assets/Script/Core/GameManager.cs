using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : Singleton<GameManager>
{

    //체력 관련
    float player_HP = 10.0f;
    public float player_HP_Max = 10.0f;

    const int startEnergy = 300;

    public float Player_HP
    {
        get => player_HP;
        set
        {
            player_HP = Mathf.Clamp(value, 0, player_HP_Max);

            refresh_HP?.Invoke();
        }
    }

    public float Player_HP_Max
    {
        get => player_HP_Max;
    }

    public Action refresh_HP;

    //에너지
    public int energy_count = 10000;

    public int Energy_Count
    {
        get => energy_count;
        set
        {
            energy_count = value;
            refresh_Energy?.Invoke();
        }
    }

    public Action refresh_Energy;

    ////진행도
    //int game_progress_current = 0;

    //public int Game_Progress_Current
    //{
    //    get => game_progress_current;
    //    set
    //    {
    //        game_progress_current = value;
    //        refresh_Progress?.Invoke();
    //    }
    //}

    //public Action refresh_Progress;


    //돈
    int money_count = 100;

    public int Money_Count
    {
        get => money_count;
        set
        {
            money_count = value;
            refresh_Money?.Invoke();
        }
    }

    public Action refresh_Money;

    int enemy_Spawn_Count = 0;

    public int Enemy_Spawn_Count
    {
        get => enemy_Spawn_Count;
        set
        {
            enemy_Spawn_Count = value;
            Enemy_Alive_Count = Enemy_Spawn_Count;
        }
    }

    int enemy_Alive_Count = 0;
    public int Enemy_Alive_Count
    {
        get => enemy_Alive_Count;
        set
        {
            enemy_Alive_Count = value;
            Debug.Log(enemy_Alive_Count);
            if(enemy_Alive_Count == 0)
            {
                Round++;
            }
        }
    }

    int round = 0;

    public int Round
    {
        get => round;
        set
        {
            round = value;
            Debug.Log("현재 라운드 : " + round);
            roundUp?.Invoke();
        }
    }

    int maxRound = 0;

    public int MaxRound
    {
        get => maxRound;
        set
        {
            maxRound = value;
        }
    }

    public Action roundUp;

    ItemDataManager itemData;
    TowerDataManager towerData;

    BuffManager buff;

    /// <summary>
    /// 아이템 데이터 메니저 (읽기 전용) 프로퍼티
    /// </summary>
    public ItemDataManager ItemData => itemData;

    /// <summary>
    /// 타워 데이터 메니저 (일기 전용) 프로퍼티
    /// </summary>
    public TowerDataManager TowerData => towerData;

    /// <summary>
    /// 버프 메니저 (읽기 전용) 프로퍼티
    /// </summary>
    public BuffManager Buff => buff;
    protected override void Initialize()
    {
        itemData = GetComponent<ItemDataManager>();
        towerData = GetComponent<TowerDataManager>();
        buff = GetComponent<BuffManager>();
        player_HP = player_HP_Max;
        Energy_Count = startEnergy;
        Enemy_Spawn_Count = 10;
        Round = 0;
        MaxRound = 0;
        refresh_HP = null;
    }
}
