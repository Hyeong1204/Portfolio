using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultAnalysis : MonoBehaviour
{
    TextMeshProUGUI[] texts;

    int allAttackCount;
    int successAttackCount;
    int failAttackCount;
    float successRatio;

    public int AllAttackCount
    {
        set
        {
            allAttackCount = value;
            texts[0].text = allAttackCount.ToString();
        }
    }

    public int SuccessAttackCount
    {
        set
        {
            successAttackCount = value;
            texts[1].text = successAttackCount.ToString();
        }
    }

    public int FailAttackCount
    {
        set
        {
            failAttackCount = value;
            texts[2].text = failAttackCount.ToString();
        }
    }

    public float SuccessRatio
    {
        set
        {
            successRatio = value;
            texts[3].text = $"{successRatio*100.0f:f1}%";
        }
    }

    private void Awake()
    {
        texts = transform.GetChild(1).GetComponentsInChildren<TextMeshProUGUI>();       // 벨류 값만 수정
    }
}
