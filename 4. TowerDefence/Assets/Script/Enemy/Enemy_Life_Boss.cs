using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Life_Boss : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        MaxHP = maxHP * 10;    // 보통 Enemy HP의 10배
    }
}
