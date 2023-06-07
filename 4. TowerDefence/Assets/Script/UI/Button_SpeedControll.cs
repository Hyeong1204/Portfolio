using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_SpeedControll : MonoBehaviour
{
    Button speed_up_button;
    Button speed_down_button;
    Manager_UI manager_UI;

    private void Awake()
    {
        speed_up_button = transform.GetChild(0).GetComponent<Button>();
        speed_down_button = transform.GetChild(1).GetComponent<Button>();

        speed_up_button.onClick.AddListener(Click_Speed_Up_Button);
        speed_down_button.onClick.AddListener(Click_Speed_Down_Button);
    }

    private void Start()
    {
        manager_UI = Manager_UI.Instance;
        speed_down_button.gameObject.SetActive(false);
        Click_Speed_Down_Button();
    }

    void Click_Speed_Up_Button()
    {
        manager_UI.SpeedUpGame();
        speed_up_button.gameObject.SetActive(false);
        speed_down_button.gameObject.SetActive(true);
    }

    void Click_Speed_Down_Button()
    {
        manager_UI.SpeedDownGame();
        speed_up_button.gameObject.SetActive(true);
        speed_down_button.gameObject.SetActive(false);
    }
}
