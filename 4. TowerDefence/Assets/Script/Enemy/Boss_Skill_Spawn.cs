using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Skill_Spawn : MonoBehaviour
{
    // 필요한 변수가 무엇인가? -> Enemy 프리팹, 지속적으로 동작을 하는 시간 간격

    public GameObject spawnPrefab_BossSkill;  // 생성할 적의 프리팹
    public float interval = 2.0f;   // 생성할 시간 간격

    Enemy Boss;
    // 필요한 기능은 무엇인가? -> Enemy 프리팹을 생성하는 코루틴
    private void Start()
    {
        Boss = GetComponent<Enemy>();
        StartCoroutine(Spawn());    // 코루틴 시작(종료는 없음)
    }

    IEnumerator Spawn()
    {
        for (int i = 0; i < 5; i++)                             // 5마리 생성
        {
            GameObject prefab = spawnPrefab_BossSkill;          // spawnPrefab_BossSkill 생성

            GameObject obj = Instantiate(prefab/*, transform*/);    // 생성하고 부모를 이 오브젝트로 설정
            //obj.transform.localPosition = Vector3.zero;
            Enemy enemy = obj.GetComponent<Enemy>();
            enemy.Setup(Boss.WayPoints, Boss.transform, Boss.CurrentIndex - 1);
            yield return new WaitForSeconds(interval);          // interval만큼 대기
        }
    }
}
