using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLoadButton : MonoBehaviour
{
    Button loadButton;

    private void Awake()
    {
        loadButton = GetComponent<Button>();
        loadButton.onClick.AddListener(GameLoad);
    }

    private void Start()
    {
        string path = $"{Application.persistentDataPath}/Save/";
        string pullPath = $"{path}data.json";

        if (!(Directory.Exists(path) && File.Exists(pullPath)))
        {
            loadButton.interactable = false;
        }
        else
        {
            loadButton.interactable = true;
        }
    }

    void GameLoad()
    {
        GameManager.Inst.GameLoad();

        SceneManager.LoadScene("main");

        UI.Instance.UISetActive(true);
    }
}
