using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class RankData : MonoBehaviour
{
    public int rankCount = 5;

    const string RankDataFolder = "Save";
    const string RankDataFileName = "Save.json";

    List<int> actionRank;
    List<float> timeRank;

    public List<int> ActionRank => actionRank;

    public List<float> TimeRank => timeRank;


    private void Awake()
    {
        actionRank= new List<int>(rankCount + 1);
        timeRank = new List<float>(rankCount + 1);
    }

    private void Start()
    {
        LoadData();

        GameManager gameManager = GameManager.Inst;
        gameManager.onGameClear += () =>
        {
            UpdateRank(gameManager.ActionCount, gameManager.PlayTime);
        };
    }

    /// <summary>
    /// ActionRank 갱신 시도. 새로운 데이터를 랭크에 추가할지 판단 후 정리
    /// </summary>
    /// <param name="actionCount">새로 추가 시도하는 행동 횟수</param>
    /// <param name="playTime">새로 추가 시도하는 플레이 타임</param>
    void UpdateRank(int actionCount, float playTime)
    {
        // 랭킹에 변화가 있으면 
        ////Debug.Log($"UpdateActionRank : {actionCount}, {playTime}");

        // 랭크에 추가
        actionRank.Add(actionCount);
        timeRank.Add(playTime);
        // 각각 소팅(정렬)
        actionRank.Sort();
        timeRank.Sort();
        // 각 리스트의 크기가 rnakCount보다 크면 마지막 노드(6등)를 제거
        if (actionRank.Count > rankCount)
        {
            actionRank.RemoveAt(rankCount);
        }
        if (TimeRank.Count > rankCount)
        {
            timeRank.RemoveAt(rankCount); 
        }

        SaveData();
    }

    /// <summary>
    /// 랭킹 정보를 파일로 정장
    /// </summary>
    void SaveData()
    {
        //Debug.Log("데이터 세이브");
        // 저장할 데이터 만들기
        JsonSaveData jsonSaveData = new();
        jsonSaveData.actionCountRank = actionRank.ToArray();
        jsonSaveData.playTimerRank = timeRank.ToArray();
        string json = JsonUtility.ToJson(jsonSaveData);

        // 폴더 있는지 확인하고 없으면 만든다.
        string path = $"{Application.dataPath}/{RankDataFolder}";
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        // 파일 저장하기
        string fullPath = $"{path}/{RankDataFileName}";
        File.WriteAllText(fullPath, json);
    }

    /// <summary>
    /// 랭킹 정보를 파일에서 불러오기
    /// </summary>
    void LoadData()
    {
        //Debug.Log("데이터 로딩");
        string path = $"{Application.dataPath}/{RankDataFolder}";
        string fullPath = $"{path}/{RankDataFileName}";

        if(Directory.Exists(path) && File.Exists(fullPath))     // 폴더와 파일 둘다 있을 때만 읽기
        {
            string json = File.ReadAllText(fullPath);
            JsonSaveData jsonSavaData = JsonUtility.FromJson<JsonSaveData>(json);
            actionRank = new List<int>(jsonSavaData.actionCountRank);
            timeRank = new List<float>(jsonSavaData.playTimerRank);

            int listSize = rankCount + 1;
            if(actionRank.Capacity != listSize)         // Capacity 크기가 다를 때 처리
            {
                if(actionRank.Capacity > listSize)      // actionRank에 listSize보다 더 많이 들어있을 때
                {
                    actionRank.RemoveRange(listSize, actionRank.Capacity - listSize);
                }
                actionRank.Capacity = listSize;
            }

            if (timeRank.Capacity != listSize)         // Capacity 크기가 다를 때 처리
            {
                if (timeRank.Capacity > listSize)      // actionRank에 listSize보다 더 많이 들어있을 때
                {
                    timeRank.RemoveRange(listSize, timeRank.Capacity - listSize);
                }
                timeRank.Capacity = listSize;
            }
        }
    }
}
