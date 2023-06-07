using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveTest : MonoBehaviour
{
    Button save;

    private void Awake()
    {
        save = GetComponent<Button>();
    }

    private void Start()
    {
        save.onClick.AddListener(GameManager.Inst.GameSave);
    }
}
