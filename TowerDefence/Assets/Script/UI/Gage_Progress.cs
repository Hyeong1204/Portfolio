using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gage_Progress : MonoBehaviour
{
    Slider progress_gage;
    Manager_UI manager_UI;

    private void Awake()
    {
        progress_gage = GetComponent<Slider>();
    }

    private void Start()
    {
        manager_UI = Manager_UI.Instance;
        progress_gage.value = manager_UI.UI_Current_Round;

        progress_gage.maxValue = manager_UI.UI_Max_Round;
        GameManager.Instance.roundUp += Refresh_Progress_Gage;
    }

    void Refresh_Progress_Gage()
    {
        progress_gage.maxValue = manager_UI.UI_Max_Round;
        progress_gage.value = manager_UI.UI_Current_Round;
    }
}
