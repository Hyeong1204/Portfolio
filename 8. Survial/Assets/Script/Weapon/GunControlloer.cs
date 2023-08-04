using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControlloer : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;         // 현재 장착된 총

    private float currentFireRate;  // 연사 속도 계산

    // 상태 변소ㅜ
    private bool isReload = false;
    private bool isFineSightMode = false;

    private AudioSource audioSource;    // 총 효과음
    
    private Vector3 originPos;          // 본래 포지션 값

    private RaycastHit hitInfo;     // 레이저 충돌 정보 받아옴

    [SerializeField]
    private Camera theCam;
    private Crosshair theCrosshair;

    [SerializeField]
    private GameObject hitEffectPrefeb;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        theCrosshair = FindObjectOfType<Crosshair>();
        originPos = Vector3.zero;
    }

    private void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryReload();
        TryFineSight();
    }

    /// <summary>
    /// 연사속도 재계산
    /// </summary>
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;
        }
    }

    /// <summary>
    /// 발사 시도
    /// </summary>
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    /// <summary>
    /// 발사 전 계산
    /// </summary>
    private void Fire()
    {
        if (!isReload)
        {
            if (currentGun.currentBulletCount > 0)
                Shoot();
            else
            {
                CancelFineSight();
                StartCoroutine(ReloadCoroutine());
            }
        }
    }

    /// <summary>
    /// 발사 후 계산
    /// </summary>
    private void Shoot()
    {
        theCrosshair.FireAnimation();
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate;      // 연사 속도 재계산
        PlaySE(currentGun.fireSound);
        currentGun.muzzleFlash.Play();
        Hit();

        // 총기 반동 코루틴 실행
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
    }

    private void Hit()
    {
        if(Physics.Raycast(theCam.transform.position, theCam.transform.forward + 
            new Vector3(UnityEngine.Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy), 
                        UnityEngine.Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),0.0f),
                        out hitInfo, currentGun.range))
        {
           var clone = Instantiate(hitEffectPrefeb, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
           Destroy(clone, 2.0f);
        }
    }

    /// <summary>
    /// 재장전 시도
    /// </summary>
    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }

    /// <summary>
    /// 사운드 재생
    /// </summary>
    /// <param name="_clip"></param>
    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    /// <summary>
    /// 재장전
    /// </summary>
    /// <returns></returns>
    IEnumerator ReloadCoroutine()
    {
        if (currentGun.carryBulletCount > 0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }
            isReload = false;
        }
        else
        {
            Debug.Log("소유한 총알이 없습니다.");
        }

    }

    /// <summary>
    /// 정조준 시도
    /// </summary>
    private void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        }
    }

    /// <summary>
    /// 정조준 취소
    /// </summary>
    public void CancelFineSight()
    {
        if (isFineSightMode)
            FineSight();
    }

    /// <summary>
    /// 정조준 로직 가동
    /// </summary>
    private void FineSight()
    {
        isFineSightMode = !isFineSightMode;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);
        theCrosshair.FineSightAnimation(isFineSightMode);

        if(isFineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FineSightActivateCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeactivateCoroutine());
        }
    }

    /// <summary>
    /// 정조준 활성화
    /// </summary>
    /// <returns></returns>
    IEnumerator FineSightActivateCoroutine()
    {
        while(currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = UnityEngine.Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }

    /// <summary>
    /// 정조준 비활성화
    /// </summary>
    /// <returns></returns>
    IEnumerator FineSightDeactivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = UnityEngine.Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }

    /// <summary>
    /// 반동 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator RetroActionCoroutine()
    {
        Vector3 recotlBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);

        if(!isFineSightMode)
        {
            currentGun.transform.localPosition = originPos;

            // 반동 시작
            while (currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recotlBack, 0.4f);
                yield return null;
            }

            // 원위치
            while(currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }

        }
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;

            // 반동 시작
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            // 원위치
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }

    public Gun GetGun()
    {
        return currentGun;
    }

    public bool GetFineSightMode()
    {
        return isFineSightMode;
    }
}
