using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultTable : MonoBehaviour
{
    TextMeshProUGUI resultText;

    private void Awake()
    {
        resultText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void SetVictory()
    {
        resultText.text = "승리!";
    }

    public void SetDefeat()
    {
        resultText.text = "패배...";
    }
}
