using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopTest : MonoBehaviour
{
    Button buyButton;
    Button saleButton;

    TextMeshProUGUI rubeCount;
    TextMeshProUGUI diaCount;

    private void Awake()
    {
        buyButton = transform.GetChild(0).GetComponent<Button>();
        buyButton.onClick.AddListener(Buy);
        saleButton = transform.GetChild(1).GetComponent<Button>();
        saleButton.onClick.AddListener(Sale);

        diaCount = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        rubeCount = transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
    }

    void Buy()
    {
        Inventory.Inst.AddItem(1, ItemEnum.Ruby);
        rubeCount.text = Inventory.Inst.inventory[(int)ItemEnum.Ruby].itemCount.ToString();
    }

    void Sale()
    {
        Inventory.Inst.SubItem(3, ItemEnum.Ruby);
        rubeCount.text = Inventory.Inst.inventory[(int)ItemEnum.Ruby].itemCount.ToString();
    }
}
