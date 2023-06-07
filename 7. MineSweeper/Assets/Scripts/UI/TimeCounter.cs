using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    ImageNumber imageNumber;
    Timer timer;

    private void Awake()
    {
        imageNumber = GetComponent<ImageNumber>();
    }

    private void Start()
    {
        timer = GetComponent<Timer>();
        GameManager gameManager = GameManager.Inst;
        timer.onTimeChange += Refresh;
    }

    private void Refresh(int count)
    {
        imageNumber.Number = count;
    }
}
