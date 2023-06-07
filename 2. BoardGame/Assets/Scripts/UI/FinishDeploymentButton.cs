using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishDeploymentButton : MonoBehaviour
{
    Button button;
    UserPlayer player;

    private void Awake()
    {
        ShipDeploymentPanel shipDeploymentpanel;
        shipDeploymentpanel = transform.parent.GetComponentInChildren<ShipDeploymentPanel>();
        shipDeploymentpanel.onDeploymentStateChange += OnComplete;

        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }


    private void Start()
    {
        player = GameManager.Inst.UserPlayer;
        foreach (var ship in player.Ships)
        {
            ship.onDeploy += OnShipDeployed;
        }



        button.interactable = false;
    }

    private void OnShipDeployed(bool isDeployed)
    {
        if(isDeployed && player.IsAllDeployed)
        {
            OnComplete();
        }
        else
        {
            NotComplete();
        }
    }

    private void OnComplete()
    {
        button.interactable = true;
    }

    private void OnClick()
    {
        GameManager.Inst.SaveShipDeployData(player);
        SceneManager.LoadScene(2);
    }

    void NotComplete()
    {
        button.interactable = false;
    }
}
