using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // 무기 중복 교체 실행 방지
    public static bool isChangeWeapon = false;
    public static Transform currentWeapon;
    public static Animator currentWeaponAnimator;

    // 무기 교체 딜레이
    [SerializeField]
    private float changeWeaponDelayTime;
    [SerializeField]
    private float changeWepaonEndDelayTime;

    // 무기 종류들
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private Hand[] hands;

    // 관리 차원에서 쉽게 무기 접근이 가능하도록 만듦
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, Hand> handDictionary = new Dictionary<string, Hand>();

    [SerializeField]
    private GunController theGuntController;
    [SerializeField]
    private HandContoroller theHandController;

    [SerializeField]
    private string currentWepaonType;

    private void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }

        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].handName, hands[i]);
        }
    }

    private void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // 무기 교체 실행 (총)
                StartCoroutine(ChangeWeaponCoroution("HAND", "맨손"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // 무기 교체 실행 (맨손)
                StartCoroutine(ChangeWeaponCoroution("GUN", "SubMachineGun1"));
            }
        }
    }

    public IEnumerator ChangeWeaponCoroution(string type, string name)
    {
        isChangeWeapon = true;
        currentWeaponAnimator.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CancelPreWeaponAction();
        WeaponChange(type, name);

        currentWepaonType = type;
        isChangeWeapon = false;
    }

    private void CancelPreWeaponAction()
    {
        switch (currentWepaonType)
        {
            case "GUN":
                theGuntController.CancelFineSight();
                theGuntController.CancelReload();
                GunController.isActivate = false;
                break;
            case "HAND":
                HandContoroller.isActivate = false;
                break;
            default:
                break;
        }
    }

    private void WeaponChange(string type, string name)
    {
        if(type == "GUN")
        {
            theGuntController.GunChange(gunDictionary[name]);
        }
        else if(type == "HAND")
        {
            theHandController.HandChange(handDictionary[name]);
        }
    }
}
