using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    // 크로스헤어 상태에 따른 총의 정확도
    private float gunAccuracy;

    // 크로스 헤어 비활성화를 위한 부모 객체
    [SerializeField]
    private GameObject crosshairHud;
    [SerializeField]
    private GunController theGunController;

    public void WalkingAnimation(bool flag)
    {
        WeaponManager.currentWeaponAnimator.SetBool("Walk", flag);
        anim.SetBool("Walking", flag);
    }

    public void RunningAnimation(bool flag)
    {
        WeaponManager.currentWeaponAnimator.SetBool("Run", flag);
        anim.SetBool("Running", flag);
    }

    public void JumpingAnimation(bool flag)
    {
        anim.SetBool("Running", flag);
    }

    public void CrouchAnimation(bool flag)
    {
        anim.SetBool("Crouching", flag);
    }

    public void FineSightAnimation(bool flag)
    {
        anim.SetBool("FineSight", flag);
    }

    public void FireAnimation()
    {
        if (anim.GetBool("Walking"))
        {
            anim.SetTrigger("Walk_Fire");
        }
        else if (anim.GetBool("Crouching"))
        {
            anim.SetTrigger("Crouch_Fire");
        }
        else
        {
            anim.SetTrigger("Idle_Fire");
        }
    }

    public float GetAccuracy()
    {
        if (anim.GetBool("Walking"))
        {
            gunAccuracy = 0.06f;
        }
        else if (anim.GetBool("Crouching"))
        {
            gunAccuracy = 0.015f;
        }
        else if (anim.GetBool("Running"))
        {
            gunAccuracy = 0.08f;
        }
        else if (theGunController.GetFineSightMode())
        {
            gunAccuracy = 0.01f;
        }
        else
        {
            gunAccuracy = 0.035f;
        }

        return gunAccuracy;
    }
}
