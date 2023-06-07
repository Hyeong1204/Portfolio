using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPopupWindow : MonoBehaviour
{
    TextMeshProUGUI notificationText;
    Button button;


    private void Awake()
    {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(Close);
        notificationText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Close();
    }

    public void Open(string text)
    {
        notificationText.text = text;

        gameObject.SetActive(true);
    }

    void Close()
    {
        gameObject.SetActive(false);
    }
}
