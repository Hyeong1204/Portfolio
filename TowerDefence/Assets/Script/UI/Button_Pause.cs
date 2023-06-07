using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Button_Pause : MonoBehaviour
{
    Button pause_button;
    Button restart_button;
    Button newGame_button;
    Button exitGame_button;
    GameObject pause_panel;
    Manager_UI manager_UI;

    private void Awake()
    {
        pause_button = transform.GetChild(0).GetComponent<Button>();
        pause_panel = transform.GetChild(1).gameObject;
        restart_button = pause_panel.transform.GetChild(1).GetComponent<Button>();
        newGame_button = pause_panel.transform.GetChild(2).GetComponent<Button>();
        exitGame_button = pause_panel.transform.GetChild(3).GetComponent<Button>();

        pause_button.onClick.AddListener(Click_Pause_Button);
        restart_button.onClick.AddListener(Click_Restart_Button);
        newGame_button.onClick.AddListener(Click_NewGame_Button);
        exitGame_button.onClick.AddListener(Click_ExitGame_Button);
    }

    private void Start()
    {
        manager_UI = Manager_UI.Instance;
        pause_panel.SetActive(false);
    }

    void Click_Pause_Button()
    {
        manager_UI.PauseGame();
        pause_panel.SetActive(true);
        pause_button.gameObject.SetActive(false);
    }

    void Click_Restart_Button()
    {
        manager_UI.ContinueGame();
        pause_panel.SetActive(false);
        pause_button.gameObject.SetActive(true);
    }
    
    void Click_NewGame_Button()
    {
        manager_UI.manager_Scene.moveScene(SceneManager.GetActiveScene().name);
    }

    void Click_ExitGame_Button()
    {
        manager_UI.manager_Scene.moveScene("MainMoveScene");
    }
}
