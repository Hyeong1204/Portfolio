using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_LockOn : Test_Base
{
    private void Start()
    {
        ItemData_EquipItem item = Gamemanager.Inst.ItemData[ItemIDCode.SliverSword] as ItemData_EquipItem;
        Gamemanager.Inst.Player.Test_AddItem(item);
        Gamemanager.Inst.Player.Test_UseItem(0);
    }
}
