using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Panel_End_Game : MonoBehaviour
{
    public GameObject panel_clear;
    public GameObject panel_over;
    Manager_UI manager_UI;
    GameManager gameManager;

    private void Awake()
    {
        manager_UI = Manager_UI.Instance;
        gameManager = GameManager.Instance;
    }

    private void Start()
    {
        gameManager.roundUp += Game_Clear;
        gameManager.refresh_HP += Game_Over;
        panel_clear.SetActive(false);
        panel_over.SetActive(false);
    }

    void Game_Clear()
    {
        if (manager_UI.UI_Current_Round == manager_UI.UI_Max_Round)
        {
            if((panel_over != null) && (!panel_over.activeSelf))
            {
                panel_clear.SetActive(true);
            }
        }
    }
    void Game_Over()
    {
        if (manager_UI.UI_Player_HP <= 0 && !panel_clear.activeSelf)
        {
            if ((panel_clear != null) && (!panel_clear.activeSelf))
            {
                panel_over.SetActive(true);
                Time.timeScale = 0.0f;
            }
        }
    }

    void Unconnect_Action()
    {
        gameManager.refresh_HP -= Game_Over;
        gameManager.roundUp -= Game_Clear;
    }


    public void Click_NewGame_Button()
    {
        manager_UI.manager_Scene.moveScene(SceneManager.GetActiveScene().name);
        Unconnect_Action();
    }

    public void Click_ExitGame_Button()
    {
        manager_UI.manager_Scene.moveScene("MainMoveScene");
        Unconnect_Action();
    }
}
