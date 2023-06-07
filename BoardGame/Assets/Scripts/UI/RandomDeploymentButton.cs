using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomDeploymentButton : MonoBehaviour
{
    UserPlayer player;
    ShipDeploymentPanel deploymentPanel;


    private void Awake()
    {
        Button autoButton = GetComponent<Button>(); ;
        autoButton.onClick.AddListener(AutoDeployment);     // 버튼 클릭함수 추가
        deploymentPanel = transform.parent.GetComponentInChildren<ShipDeploymentPanel>();
    }

    private void Start()
    {
        player = GameManager.Inst.UserPlayer;
    }

    private void AutoDeployment()
    {
        if(player.IsAllDeployed)                // 함선이 이미 모두 배치 되었으면
        {
            player.UndoAllShipDeployment();     // 배치 전부 취소
        }
        player.AutoShipDeployment(true);        // 함선 자동배치 실행
        deploymentPanel.SetToggleSelectAll();   // 토글버튼 전부 눌려진 상태로 만들기
    }
}
