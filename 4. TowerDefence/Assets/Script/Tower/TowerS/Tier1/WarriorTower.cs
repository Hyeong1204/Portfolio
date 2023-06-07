using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorTower : Tower
{
    public Sprite[] sprite;
    SpriteRenderer spriteRenderer;


    protected override void Awake()
    {
        base.Awake();
        towerUpgradeCost = new int[] { 100, 200, 300 };
        isphysics = true;
        isAttackFly = false;
        type = TowerType.Warrior;

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public override void towerUpgrade()
    {
        if (GameManager.Instance.Energy_Count > towerUpgradeCost[level-1] && level <maxTowerLevel)
        {
            level++;
            attackDamage += 10;
            spriteRenderer.sprite = sprite[level - 2];
            GameManager.Instance.Energy_Count -= towerUpgradeCost[level - 1];
        }
        else
        {
            Debug.Log("업그레이드 실패");
        }

        Debug.Log($"{level}   {attackDamage}");
    }


}
