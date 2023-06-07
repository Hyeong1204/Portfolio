using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemMaterial : MonoBehaviour
{
    public Image image;
    TextMeshProUGUI eaText;

    public Color okColor;
    public Color cancelColor;

    private void Awake()
    {
        image = GetComponentInChildren<Image>();
        eaText= GetComponentInChildren<TextMeshProUGUI>();
    }

    /// <summary>
    /// 강화창 재료 수량 변경
    /// </summary>
    /// <param name=""></param>
    public void TextChange(int present, int necessity)
    {

        Color color;
        if(present >= necessity)
        {
            // 가지고 있는 개수가 같거나 많다
            color = okColor;
        }
        else
        {
            // 가지고 있는 개수가 적다
            color = cancelColor;
        }


        string colorCode = ColorUtility.ToHtmlStringRGB(color);

        eaText.text = $"(<#{colorCode}>{present}</color> / {necessity})";
    }
}
