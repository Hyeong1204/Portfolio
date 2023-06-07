using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    Button myButton;

    private void Awake()
    {
        myButton = GetComponent<Button>();
    }

    private void Start()
    {
        GameManager.Inst.gameStart = () => myButton.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
