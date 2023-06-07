using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    UserPlayer player;
    EnemyPlayer enemy;

    Button toggleButton;
    Button reStart;
    bool isOpenTable = true;

    ResultTable table;
    ResultAnalysis userAnalysis;
    ResultAnalysis enemyAnalysis;

    private void Awake()
    {
        table = GetComponentInChildren<ResultTable>();

        toggleButton = transform.GetChild(0).GetComponent<Button>();
        toggleButton.onClick.AddListener(ToggleTable);

        reStart = transform.GetChild(1).GetChild(3).GetComponent<Button>();
        reStart.onClick.AddListener(ReStart);

        userAnalysis = transform.GetChild(1).GetChild(1).GetComponent<ResultAnalysis>();
        enemyAnalysis = transform.GetChild(1).GetChild(2).GetComponent<ResultAnalysis>();
    }

    private void Start()
    {
        GameManager gamaManger = GameManager.Inst;

        player = gamaManger.UserPlayer;
        player.onDefeat += (_) =>
        {
            Open();
            table.SetDefeat();
        };

        enemy = gamaManger.EnemyPlayer;
        enemy.onDefeat += (_) =>
        {
            Open();
            table.SetVictory();
        };

        Close();
    }


    private void ToggleTable()
    {
        if (isOpenTable)
        {
            table.Close();
        }
        else
        {
            table.Open();
        }

        isOpenTable = !isOpenTable;
    }

    private void ReStart()
    {
        SceneManager.LoadScene(1);
    }

    void Open()
    {
        gameObject.SetActive(true);
        StartCoroutine(OpenDelay());
    }

    void Close()
    {
        gameObject.SetActive(false);
    }

    IEnumerator OpenDelay()
    {
        yield return null;

        userAnalysis.AllAttackCount = player.FailAttackCount + player.SuccessAttackCount;
        userAnalysis.SuccessAttackCount = player.SuccessAttackCount;
        userAnalysis.FailAttackCount = player.FailAttackCount;
        userAnalysis.SuccessRatio = (float)player.SuccessAttackCount / (player.FailAttackCount + player.SuccessAttackCount);

        enemyAnalysis.AllAttackCount = enemy.FailAttackCount + enemy.SuccessAttackCount;
        enemyAnalysis.SuccessAttackCount = enemy.SuccessAttackCount;
        enemyAnalysis.FailAttackCount = enemy.FailAttackCount;
        enemyAnalysis.SuccessRatio = (float)enemy.SuccessAttackCount / (enemy.FailAttackCount + enemy.SuccessAttackCount);
    }
}
