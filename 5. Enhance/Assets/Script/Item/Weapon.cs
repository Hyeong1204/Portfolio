using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public int enganceRange = 0;       // 강화 수치
    Image weaponImage;


    TotalWeapon model;

    public TotalWeapon Model => model;
    public WeaponData WeaponModel => model.WeaponDatas[EnganceRange];
    public WeaponData NextWeaponModel => model.WeaponDatas[EnganceRange + 1 < model.WeaponDatas.Length ? EnganceRange + 1 : EnganceRange];

    public Action<int> enhanceSuccess;  // 강화 성공
    public Action<int> enhanceFail;     // 강화 실패
    public Action<int> enganceDestroy;  // 무기 파괴
    public Action lack;     // 재료가 부족할 때 


    public int EnganceRange
    {
        get => enganceRange;
        set
        {
            if (enganceRange != value)
            {
                if (model != null)
                {
                    if (model.WeaponDatas.Length > enganceRange)
                    {
                        enganceRange = value;
                        gameObject.name = $"{WeaponModel.name}";
                        if (weaponImage != null)
                            weaponImage.sprite = WeaponModel.itemIcon;
                    }
                }
                else
                {
                    model = GameManager.Inst.WeaponManager.weaponItems[0];
                    EnganceRange = value;
                }
            }
        }
    }

    private void Awake()
    {
        weaponImage = GetComponent<Image>();
    }

    private void Start()
    {
        GameManager gameManager = GameManager.Inst;

        model = gameManager.WeaponManager.weaponItems[0];

        bool result = true;
        for (int i = 0; i < gameManager.Weapons.Length; i++)
        {
            if (gameManager.Weapons[i].EnganceRange > 0)
            {
                result = false;
                break;
            }
        }

        if (result)
        {
            Rest();
        }

    }

    public void Rest()
    {
        EnganceRange = 0;
        gameObject.name = $"{WeaponModel.name}";
    }

    public void Enhance()
    {
        if (model.WeaponDatas.Length - 1 > enganceRange)
        {
            // 최고 단계인지 검사
            if (MaterialInspection())
            {
                // 재료가 충분한지 검사
                float rand = UnityEngine.Random.Range(0.0f, 1.0f);
                if (WeaponModel.enforceRatio > rand)
                {
                    // 강화 성공
                    EnhanceSuccess();
                    Debug.Log($"강화 성공! 현재 강화 단계 : {EnganceRange}");
                }
                else if (!(WeaponModel.destroy <= 0.0f) && WeaponModel.DestroyRatio > rand)
                {
                    // 파괴
                    EnganceDestroy();
                    Debug.Log($"무기가 파괴 되어 0강으로 돌아갑니다. 현재 강화 단계 : {EnganceRange}");
                }
                else
                {
                    // 강화 실패 (하락)
                    if (EnganceRange != 0)
                    {
                        EnhanceFail();
                        Debug.Log($"강화 실패! 현재 강화 단계 : {EnganceRange}");
                    }
                    else
                    {
                        Debug.Log($"최하 단계이므로 하락 하지않습니다. : {enganceRange}");
                    }
                }

                UI.Instance.HP--;
            }
        }
        else
        {
            Debug.Log($"현재 최고 단계입니다.");
        }
    }

    void EnhanceSuccess()
    {
        EnganceRange++;
        enhanceSuccess?.Invoke(EnganceRange);
    }

    void EnhanceFail()
    {
        EnganceRange--;
        enhanceFail?.Invoke(EnganceRange);
    }

    void EnganceDestroy()
    {
        EnganceRange = 0;
        enganceDestroy?.Invoke(EnganceRange);
    }

    /// <summary>
    /// 인벤토리에서 재료가 충분한지 검사
    /// </summary>
    /// <returns></returns>
    bool MaterialInspection()
    {
        bool result = false;         // false로 수정해야함

        if (WeaponModel.costData.Length != 0)
        {
            // 강화 재료가 하나라도 필요하면
            int index = WeaponModel.costData.Length;
            Inventory inven = Inventory.Inst;
            for (int i = 0; i < index; i++)
            {
                if (WeaponModel.costData[i].costItemValue <= inven.inventory[(int)WeaponModel.costData[i].costItem].itemCount)
                {
                    result = true;
                }
                else
                {
                    result = false;
                    Debug.Log("재료가 부족합니다.");
                    lack?.Invoke();
                    break;
                }
            }

            if (result)
            {
                // 모든 재료가 있다면 해당 재료를 인벤토리에서 감소
                for (int i = 0; i < index; i++)
                {
                    inven.SubItem(WeaponModel.costData[i].costItemValue, WeaponModel.costData[i].costItem);
                }
            }
        }
        else
        {
            result = true; // 강화 재료가 필요가 없다.
        }

        return result;
    }
}
