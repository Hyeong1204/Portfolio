using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Life_Down : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        MaxHP = maxHP / 2;    // hp 절반
    }
}
