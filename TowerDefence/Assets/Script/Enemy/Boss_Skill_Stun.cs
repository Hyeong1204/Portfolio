using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Skill_Stun : MonoBehaviour
{
    [Header("스턴 지속 시간")]
    public float stunTime = 2.0f;   

    public BuffType type;
    Tower[] towersGameObject;

    BuffManager buffManager;

    private void Start()
    {
        buffManager = GameManager.Instance.Buff;
        StartCoroutine(TowerStun());
    }

    public IEnumerator TowerStun()
    {
        while (true)
        {
            yield return new WaitForSeconds(10.0f);
            towersGameObject = GameObject.FindObjectsOfType<Tower>();
            foreach (var Tower in towersGameObject)
            {
                buffManager.CreateBuff(type, 0, stunTime, Tower);                 // 타워한테 스턴 적용
            }
            Debug.Log("타워 스턴");
        }
    }
}
