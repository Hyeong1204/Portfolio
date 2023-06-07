using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleExit : MonoBehaviour
{
    Button exit;

    private void Awake()
    {
        exit = transform.GetComponent<Button>();
        exit.onClick.AddListener(ExitButton);
    }

    void ExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;    // player모드를 false로
#else
        Application.Quit();
#endif
    }
}
