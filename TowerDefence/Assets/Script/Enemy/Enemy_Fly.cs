using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Fly : Enemy
{
    bool isFly = true; // 공중 유닛인지 아닌지

    public bool IsFly   // 타워랑 호환할 프로퍼티 생성
    {
        get { return isFly; }
        set { isFly = value; }
    }
}
