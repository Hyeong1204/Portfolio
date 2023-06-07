using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Speed_Up : Movement
{
    private void Awake()
    {
        MoveSpeed = moveSpeed * 2;  // 이동속도 2배
    }
}
