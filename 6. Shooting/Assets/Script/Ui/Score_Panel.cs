using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;

public class Score_Panel : MonoBehaviour
{
    TextMeshProUGUI scoreText;
    int targetScore = 0;
    float currentScore = 0.0f;
    public float ScoreTime = 150.0f;

    private void Awake()
    {
        scoreText = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();

    }

    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        player.onScoreChange += SetTargetScore;

        targetScore = 0;
        currentScore = 0.0f;
        scoreText.text = "0";
    }

    private void Update()
    {
        if(currentScore < targetScore)
        {
            currentScore += Time.deltaTime * ScoreTime;

            currentScore = Mathf.Min(currentScore, targetScore);        //currentScore가 targetScore보다 무조건 작거나 같도록 변경.
            scoreText.text = $"{currentScore:f0}";
        }
    }

    private void SetTargetScore(int totalScore)
    {
        //socreText.text = totalScore.ToString();
        //scoreText.text = $"{totalScore,4}";

        targetScore = totalScore;
    }
}
