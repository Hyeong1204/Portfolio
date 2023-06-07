using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Inventory : Test_Base
{
    Inventory inven;
    InventoryUI inventoryUI;

    private void Start()
    {
        inven = new Inventory(null);
        inventoryUI = FindObjectOfType<InventoryUI>();
        inventoryUI.InitializeInventoty(inven);
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        inven.AddItem(ItemIDCode.Ruby);
        inven.AddItem(ItemIDCode.Emerald);
        inven.AddItem(ItemIDCode.Sapphire);
        inven.AddItem(ItemIDCode.Ruby);
        inven.AddItem(ItemIDCode.Emerald);
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        inven.PrintInventory();
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        Test_AddItemForUI();
    }

    protected override void Test4(InputAction.CallbackContext _)
    {
        inven.MoveItem(0, 3);
    }

    protected override void Test5(InputAction.CallbackContext _)
    {
        inven.AddItem(ItemIDCode.Ruby, 9);
        inven.AddItem(ItemIDCode.Emerald, 8);
        inven.AddItem(ItemIDCode.Sapphire, 20);
    }

    void Test_AddItemForUI()
    {
        inven.ClearInventory();
        inven.AddItem(ItemIDCode.Ruby);
        inven.AddItem(ItemIDCode.Ruby);
        inven.AddItem(ItemIDCode.Ruby);
        inven.AddItem(ItemIDCode.Ruby);
        inven.AddItem(ItemIDCode.Emerald);
        inven.AddItem(ItemIDCode.Emerald);
        inven.AddItem(ItemIDCode.Sapphire);

        inven.AddItem(ItemIDCode.Ruby, 5);
        inven.AddItem(ItemIDCode.Ruby, 5);
        inven.AddItem(ItemIDCode.Ruby, 5);
        inven.AddItem(ItemIDCode.Ruby, 5);
        inven.AddItem(ItemIDCode.Ruby, 5);
    }

}
