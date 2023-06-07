using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    Button menu;

    Image menuPanel;
    Image nothingPanel;
    Button save;
    Button exit;

    private void Awake()
    {
        menu = transform.GetComponent<Button>();
        menu.onClick.AddListener(OpenPanel);

        menuPanel = transform.GetChild(0).GetComponent<Image>();

        nothingPanel = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>();

        save = transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<Button>();
        save.onClick.AddListener(SaveButton);
        exit = transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<Button>();
        exit.onClick.AddListener(ExitButton);
    }

    private void Start()
    {
        menuPanel.gameObject.SetActive(false);
    }

    void OpenPanel()
    {
        menuPanel.gameObject.SetActive(true);

    }

    void SaveButton()
    {
        GameManager.Inst.GameSave();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;    // player모드를 false로
#else
        Application.Quit();
#endif
    }

    void ExitButton()
    {
        menuPanel.gameObject.SetActive(false);
    }
}
