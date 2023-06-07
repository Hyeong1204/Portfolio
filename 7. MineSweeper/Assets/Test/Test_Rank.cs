using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SortTest : IComparable
{
    public int a;
    public float b;

    public SortTest(int _a, int _b)
    {
        a = _a;
        b = _b;
    }

    public int CompareTo(object obj)
    {
        SortTest sort = obj as SortTest;
        if(a < sort.a)
        {
            return -1;
        }
        else if( a > sort.a)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public void Print()
    {
        //Debug.Log($"");
    }
}


public class Test_Rank : TestBase
{
    protected override void Test1(InputAction.CallbackContext _)
    {
        List<int> list = new List<int>();
        list.Add(20);
        list.Add(10);
        list.Add(40);
        list.Add(30);
        list.Add(50);

        list.Sort();
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        
    }
}
