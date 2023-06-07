using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageTower : MagicianTower
{
    protected override void Awake()
    {
        base.Awake();
        towerUpgradeCost = new int[] { 50, 100, 150 };
        isphysics = false;
        isAttackFly = true;

        type = TowerType.Mage;
    }

    public override void towerUpgrade()
    {
        if (GameManager.Instance.Energy_Count > towerUpgradeCost[level - 1] && level < maxTowerLevel)
        {
            level++;
            attackDamage += 10;
            GameManager.Instance.Energy_Count -= towerUpgradeCost[level - 1];
        }
        else
        {
            Debug.Log("업그레이드 실패");
        }

        Debug.Log($"{level}   {attackDamage}");
    }

    protected override void Attack()
    {
        target = Enemys[0].transform; //리스트의 첫번째 적에게 공격
        Vector3 targetDir = (target.position - transform.position).normalized; // 방향을 구한후
        float angle = Vector2.SignedAngle(Vector2.right, targetDir); // 각도 구하기

        GameObject obj = Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0, 0, angle))); // 총알을 적의 방향으로 생성
        Bullet bull = obj.GetComponent<Bullet>();
        bull.Damage = attackDamage; // 총알에 데메지 전달
        bull.IsPhysics = isphysics; // 총알에 물리/마법 속성 전달
        bull.isSlowAttack = true; // 슬로우총알 발사
        bull.Target = Enemys[0];

        obj.transform.parent = this.transform; // 총알을 타워의 자식으로 설정
    }
}
