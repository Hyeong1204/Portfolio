using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Battle : Test_Base
{
    Player player;
    public bool noise = false;
    public int count = 0;

    private void Start()
    {
        player = Gamemanager.Inst.Player;
    }

    protected override void Test1(InputAction.CallbackContext obj)
    {
        player.Defence(60.0f);
    }

    protected override void Test2(InputAction.CallbackContext obj)
    {
        player.HP = player.MaxHP;
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        ItemFactory.MakeItem(ItemIDCode.Ruby, new Vector3(1, 0, 1), noise);
    }

    protected override void Test4(InputAction.CallbackContext _)
    {
        ItemFactory.MakeItems(ItemIDCode.Ruby, count, new Vector3(1, 0, 1), noise);
    }

    protected override void Test5(InputAction.CallbackContext _)
    {
        ItemFactory.MakeItems(ItemIDCode.Emerald, count);
    }
}
