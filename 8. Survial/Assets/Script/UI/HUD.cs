using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private GunController theGunGontroller;
    private Gun currentGun;

    // 필요하면 HUD 호출, 필요 없으면 HUD 비활성화 
    [SerializeField]
    private GameObject bulletHUD;

    // 총알 개수 수정 반영 텍스트
    [SerializeField]
    private TextMeshProUGUI[] textBullet;

    private void Update()
    {
        CheckBullet();
    }

    private void CheckBullet()
    {
        currentGun = theGunGontroller.GetGun();
        textBullet[0].text = currentGun.currentBulletCount.ToString();
        textBullet[1].text = currentGun.reloadBulletCount.ToString();
        textBullet[2].text = currentGun.carryBulletCount.ToString();
    }
}
