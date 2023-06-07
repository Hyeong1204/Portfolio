using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Show_Item : MonoBehaviour
{
    public ItemDataManager itemDataManager;
    public GameObject item_button;
    public Button item_in;
    public Button item_out;
    public GameObject item_panel;

    Button_Item[] button_Items;
    GameObject item_list;

    private void Awake()
    {
        item_list = transform.GetChild(0).GetChild(1).gameObject;
        Create_Item_Buttons();
        button_Items = item_list.GetComponentsInChildren<Button_Item>();
        item_out.onClick.AddListener(Out_Item);
        item_in.onClick.AddListener(In_Item);
    }

    private void Start()
    {
        for (int i = 0; i < button_Items.Length; i++)
        {
            button_Items[i].ItemInitialize((uint)i);
            button_Items[i].StartButton();
            button_Items[i].Refresh();
            button_Items[i].ParentTransform(item_panel.transform.GetChild(0));
        }
    }

    void Create_Item_Buttons()
    {
        for(int i = 0; i < itemDataManager.itemData.Length; i++)
        {
            GameObject item = Instantiate(item_button);
            item.transform.SetParent(item_list.transform);
        }
    }

    void Out_Item()
    {
        item_panel.SetActive(true);
        item_out.gameObject.SetActive(false);
    }
    void In_Item()
    {
        item_panel.SetActive(false);
        item_out.gameObject.SetActive(true);
    }
}
