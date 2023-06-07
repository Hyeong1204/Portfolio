using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Life_Up : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        MaxHP = maxHP * 2;    // hp 2배
    }
}
