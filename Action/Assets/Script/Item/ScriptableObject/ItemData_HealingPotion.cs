using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Healing Potion", menuName = "Scriptable Object/Item Data - Healiing Potion", order = 2)]
public class ItemData_HealingPotion : ItemData, IUsable
{
    [Header("힐링 포션 데이터")]
    public float healPoint = 20.0f;

    /// <summary>
    /// 아이템을 사용하면 일어날 일 처리
    /// </summary>
    /// <param name="target">이 아이템의 사용효과를 받을 대상의 게임오브젝트</param>
    public bool Use(GameObject target = null)
    {
        bool result = false;
        IHealth health = target.GetComponent<IHealth>();
        if (health != null)
        {
            float oldHP = health.HP;
            health.HP += healPoint;

            Debug.Log($"{itemName}을 사용했습니다. HP가 {healPoint}만큼 증가합니다. HP : {oldHP} -> {health.HP}");
            result = true;
        }
        return result;
    }
}
