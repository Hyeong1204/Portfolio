using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Skill_HP : MonoBehaviour
{
    public GameObject gameobjForHP;         // Enemy 프리팹 가져오기

    private void Awake()
    {
        StartCoroutine(HpHeal());
    }

    IEnumerator HpHeal()
    {
        Enemy enemy = GetComponent<Enemy>();
        while (true)
        {
            yield return new WaitForSeconds(10.0f);                        // 10초마다
            enemy.Hp += (enemy.MaxHP / 4);    // 최대 hp의 4분의1 회복
            Debug.Log("HP가 MaxHP/4만큼 회복");
        }
    }
}
