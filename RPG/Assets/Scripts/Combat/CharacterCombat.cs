using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    public event Action OnIdle;
    public event Action OnAttack;
    public event Action OnHitted;
    public event Action OnDie;

    CharacterStat myStat;

    

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
    }
      
    private void Update()
    {
        attackCoolTime -= Time.deltaTime;

        if(Time.time - lastAttackTime > coolTime)
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
                enemyStat.Hitted(myStat.power);
                enemyStat.GetComponent<CharacterCombat>().Hitted();
            }
            isInCombat = true;
            attackCoolTime = coolTime;
            lastAttackTime = Time.time;
        }
    }

    public void Hitted()
    {
        OnHitted?.Invoke();
    }
}
