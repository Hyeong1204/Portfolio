using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Skill_DamageDown : MonoBehaviour
{
    [Range(0f, 1f)]
    public float reduceDamage;          // 데미지 감소율
    public float skillCoolTime;         // 스킬 쿨타임
    public float skillDuration;         // 스킬 지속시간

    BuffType tpye = BuffType.PowerDown;
    BuffManager buffManager;

    WaitForSeconds time;

    private void Start()
    {
        time = new WaitForSeconds(skillCoolTime);                   // 코루틴용 시간 설정
        buffManager = GameManager.Instance.Buff;
        StartCoroutine(SkillStart());
    }

    IEnumerator SkillStart()
    {
        while (true)
        {
            Tower[] towers = FindObjectsOfType<Tower>();            // 모든 타워 가져오기

            yield return time;                                      // time 시간 동안

            foreach (var tower in towers)                           // 모든 타워한테 디버프 주기
            {
                buffManager.CreateBuff(tpye, reduceDamage, skillDuration, tower);      // 해당 타워에게 적용
            }
        }
    }
}
