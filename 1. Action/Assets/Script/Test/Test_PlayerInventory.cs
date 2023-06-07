using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_PlayerInventory : Test_Base
{
    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        player.Money += 1000000;   
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        player.Money = 1; 
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        
    }
}
