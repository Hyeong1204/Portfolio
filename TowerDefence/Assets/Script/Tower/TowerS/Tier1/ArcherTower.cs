using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ArcherTower : Tower
{

    public Sprite[] sprite;
    SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        towerUpgradeCost = new int[] { 50, 100, 150 };
        isphysics = true;
        isAttackFly = true;
        type = TowerType.Archer;

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }



    public override void towerUpgrade()
    {
        if (GameManager.Instance.Energy_Count > towerUpgradeCost[level - 1] && level < maxTowerLevel)
        {
            level++;
            attackDamage += 10;
            spriteRenderer.sprite = sprite[level-2];
            GameManager.Instance.Energy_Count -= towerUpgradeCost[level - 1];
        }
        else
        {
            Debug.Log("업그레이드 실패");
        }

        Debug.Log($"{level}   {attackDamage}");
    }

    
}
