using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStartButton : MonoBehaviour
{
    Button startButton;

    private void Awake()
    {
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(GameStart);
    }

    void GameStart()
    {
        string path = $"{Application.persistentDataPath}/Save/";
        string pullPath = $"{path}data.json";

        if (Directory.Exists(path) && File.Exists(pullPath))
        {
            File.Delete(pullPath);
        }

        GameManager.Inst.GameLoad();

        SceneManager.LoadScene("main");

        UI.Instance.UISetActive(true);
    }
}
