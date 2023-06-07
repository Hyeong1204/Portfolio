using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBase : MonoBehaviour
{
    public BuffType buffType;       // 버프 타입
    public float percentage;        // 변화량
    public float duration;          // 지속시간
    public float currentTime;

    Tower tower;
    Enemy enemy;

    WaitForSeconds seconds = new WaitForSeconds(0.1f);

    /// <summary>
    /// 초기화 작업(타워)
    /// </summary>
    /// /// <param name="type">버프 유형</param>
    /// <param name="value">버프의 증감량(% 유형)</param>
    /// <param name="time">지속시간</param>
    /// <param name="enemy">버프 효과를 받을 타워</param>
    public void Initialize(BuffType type, float value, float time, Tower tower)
    {
        buffType = type;                // 버프 타입변경
        percentage = value;             // 버프 효과 수치 변경
        duration = time;                // 버프 지속시간 변경
        currentTime = duration;         // currentTime을 duration로 변경
        if (tower != null)              // 타워가 있다면
        {
            this.tower = tower;         
            tower.onBuff.Add(this);     // 그 타워에 버프리스트 추가
            tower.ChooseBuff(type);     // 그 타워에 버프 적용
        }
        Execute();
    }

    /// <summary>
    /// 초기화 작업(적)
    /// </summary>
    /// <param name="type">버프 유형</param>
    /// <param name="value">버프의 증감량(% 유형)</param>
    /// <param name="time">지속시간</param>
    /// <param name="enemy">버프 효과를 받을 적</param>
    public void Initialize(BuffType type, float value, float time, Enemy enemy)
    {
        buffType = type;
        percentage = value;
        duration = time;
        currentTime = duration;
        if (enemy != null)
        {
            this.enemy = enemy;
            enemy.onBuff.Add(this);
            enemy.ChooseBuff(type);
        }
        Execute();
    }

    /// <summary>
    /// 버프의 효과를 적용하고 타이머를 적용하는 함수
    /// </summary>
    protected virtual void Execute()
    {
        // 타워 리스트 추가
        // 타워에 ChooseBuff 함수 호출
        StartCoroutine(Activation());
    }

    /// <summary>
    /// 지속시간 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator Activation()
    {
        while (currentTime > 0)
        {
            currentTime -= 0.1f;
            yield return seconds;
        }
        currentTime = 0;
        Deactivaltion();
    }

    /// <summary>
    /// 타이머가 끝나면 버프 제거
    /// </summary>
    void Deactivaltion()
    {
        if (tower != null)
        {
            // 타워 일때
            tower.onBuff.Remove(this);
            tower.ChooseBuff(buffType);
        }
        else
        {
            // 적 일때
            if(enemy != null)
            {
                enemy.onBuff.Remove(this);
                enemy.ChooseBuff(buffType);
            }
        }
        // 타워 리스트 지우고
        // 함수 다시한번 호출
        Destroy(gameObject);
    }
}
