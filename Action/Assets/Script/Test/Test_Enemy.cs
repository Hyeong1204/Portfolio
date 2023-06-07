using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Enemy : Test_Base
{
    public Slime enemy;

    protected override void Test1(InputAction.CallbackContext obj)
    {
        enemy.Test();
    }
}
