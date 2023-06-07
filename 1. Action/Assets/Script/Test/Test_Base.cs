using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Base : MonoBehaviour
{
    PlayerInputAction input;

    private void Awake()
    {
        input = new PlayerInputAction();
    }

    protected virtual void OnEnable()
    {
        input.Test.Enable();
        input.Test.Test1.performed += Test1;
        input.Test.Test2.performed += Test2;
        input.Test.Test3.performed += Test3;
        input.Test.Test4.performed += Test4;
        input.Test.Test5.performed += Test5;
    }

    protected virtual void OnDisable()
    {
        input.Test.Test5.performed -= Test5;
        input.Test.Test4.performed -= Test4;
        input.Test.Test3.performed -= Test3;
        input.Test.Test2.performed -= Test2;
        input.Test.Test1.performed -= Test1;
        input.Test.Disable();
    }

    protected virtual void Test1(InputAction.CallbackContext _)
    {
        
    }

    protected virtual void Test2(InputAction.CallbackContext _)
    {

    }

    protected virtual void Test3(InputAction.CallbackContext _)
    {

    }

    protected virtual void Test4(InputAction.CallbackContext _)
    {

    }

    protected virtual void Test5(InputAction.CallbackContext _)
    {

    }
}
