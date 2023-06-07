using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public WeaponData weapon;

    private void Start()
    {
        for (int i = 0; i < 10000; i++)
        {
            Enhance();
        }
    }

    public void Enhance()
    {
        // 재료가 충분한지 검사
        float rand = UnityEngine.Random.Range(0.0f, 1.0f);
        if (weapon.enforceRatio > rand)
        {
            // 강화 성공
            Debug.Log($"강화 성공!");
        }
        else if (!(weapon.destroy <= 0.0f) && weapon.DestroyRatio > rand)
        {
            // 파괴
            Debug.Log($"무기가 파괴");
        }
        else
        {
            Debug.Log($"강화 실패!");

        }
    }
}
