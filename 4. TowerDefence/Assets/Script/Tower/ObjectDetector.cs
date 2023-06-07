using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
    public TowerSpwaner towerSpwaner;

    private Camera mainCamera;
    private Ray2D ray;
    private RaycastHit hit;

    private PlayerInputAction controller;


    public GameObject selectTile;
    public GameObject selectTower;

    bool isTileSelect = false; // 타일이 골라진다면
    bool isPointerOverGameObject = false;

    bool isTowerSelect = false;

    public bool IsTileSelect
    {
        get { return isTileSelect; }
        set
        {
            isTileSelect = value;
        }
    }



    public bool IsTowerSelect
    {
        get { return isTowerSelect; }
        set
        {
            isTowerSelect = value;
        }
    }


    private void Awake()
    {
        mainCamera = Camera.main;
        controller = new PlayerInputAction();
    }

    private void OnEnable()
    {
        controller.Enable();
        controller.Player.Build.performed += DetectTile;
        controller.Player.Build.canceled += DetectTile;
    }

    private void OnDisable()
    {
        controller.Player.Build.canceled -= DetectTile;
        controller.Player.Build.performed -= DetectTile;
        controller.Disable();
    }

    private void Update()
    {
        if(EventSystem.current.IsPointerOverGameObject() == false)
        {
            isPointerOverGameObject = false;
        }
        else
        {
            isPointerOverGameObject = true;
        }
    }


    /// <summary>
    /// 타일 확인
    /// </summary>
    /// <param name="context"></param>
    private void DetectTile(InputAction.CallbackContext context)
    {
        if (!isTileSelect) // 선택된 타일이 없다면
        {
            if (context.performed && !isPointerOverGameObject) // 마우스가 눌릴때 && 마우스가 UI를 가리키지 않았을 때
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                Debug.Log(pos);
                ray = new Ray2D(pos, Vector2.zero);
                RaycastHit2D[] hit2Ds = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);

                foreach(RaycastHit2D hit2D in hit2Ds)
                if (hit2D.collider != null) //광선에 부딪히는 오브젝트 검출
                {
                    if (hit2D.transform.CompareTag("Tower") && hit2D.collider.gameObject.layer == LayerMask.NameToLayer("Tower"))
                    {
                        selectTower = hit2D.collider.gameObject;
                        IsTowerSelect = true; // 타일이 선택되었다고 알려둠
                        Debug.Log("타워 선택");
                    }
                    if (hit2D.transform.CompareTag("WallTile") && !isTileSelect) //WallTile이면
                    {
                        //towerSpwaner.SpawnTower(hit.transform); // 타워 설치
                        selectTile = hit2D.collider.gameObject; // 타일에 저장
                        
                        IsTileSelect = true; // 타일이 선택되었다고 알려둠
                        Debug.Log("타일 선택");
                    }

                }
            }
        }
    }
}
