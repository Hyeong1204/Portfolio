using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterCombat : MonoBehaviour
{
    public event Action OnIdle;
    public event Action OnAttack;
    public event Action OnHitted;
    public event Action OnDie;

    CharacterStat myStat;

    public Transform hpBarTf;

    #region CoolTime
    const float coolTime = 1.0f;
    public float attackCoolTime = 0.0f;
    //public float attackSpeed = 1.0f;
    //public float attackDelay = 1.0f;
    float lastAttackTime;
    public bool isInCombat = false;
    #endregion

    private void Awake()
    {
        myStat = GetComponent<CharacterStat>();
        HpBarManager.instance.Create(hpBarTf, myStat);
    }

    private void Update()
    {
        attackCoolTime -= Time.deltaTime;

        if (Time.time - lastAttackTime > coolTime)
        {
            isInCombat = false;
        }
    }

    public void Attack(CharacterStat enemyStat)
    {
        //if (OnAttack != null)
        //{
        //    OnAttack();
        //}

        if (attackCoolTime < 0f)
        {
            if (OnAttack != null)
            {
                OnAttack();

                enemyStat.GetComponent<CharacterCombat>().Hitted();
                StartCoroutine(GetDamage(enemyStat, 0.5f));
            }
            isInCombat = true;
            attackCoolTime = coolTime;
            lastAttackTime = Time.time;
        }
    }

    IEnumerator GetDamage(CharacterStat enemyStat, float dalay)
    {
        yield return new WaitForSeconds(dalay);
        if (enemyStat != null)
        {
            enemyStat.Hitted(myStat.power);
        }
        else
        {
            Idle();
        }
    }

    public void Hitted()
    {
        OnHitted?.Invoke();
    }

    public void Idle()
    {
        OnIdle?.Invoke();
    }
}
