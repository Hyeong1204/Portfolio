using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    PlayerInputAction input;

    private void Awake()
    {
        input = new PlayerInputAction();
    }

    private void OnEnable()
    {
        input.Title.Enable();
        input.Title.AnyKey.performed += GameStart;
    }

    private void OnDisable()
    {
        input.Title.AnyKey.performed -= GameStart;
        input.Title.Disable();
    }

    private void GameStart(InputAction.CallbackContext _)
    {
        SceneManager.LoadScene(1);      // 씬 바꾸기
    }
}
