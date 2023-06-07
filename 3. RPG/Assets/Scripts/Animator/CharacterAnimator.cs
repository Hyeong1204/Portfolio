using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimator : MonoBehaviour
{
    // 캐릭터들의 애니메이터 파라미터를 조절하는 부분
    CharacterCombat combat;
    Animator an;
    NavMeshAgent agent;

    private void Awake()
    {
        an = GetComponentInChildren<Animator>();
        combat = GetComponent<CharacterCombat>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        combat.OnIdle += OnIdle;
        combat.OnAttack += OnAttack;
        combat.OnHitted += OnHitted;
        combat.OnDie += Die;
    }

    private void OnDisable()
    {
        combat.OnDie -= Die;
        combat.OnHitted -= OnHitted;
        combat.OnAttack -= OnAttack;
        combat.OnIdle -= OnIdle;
    }

    private void Update()
    {
        an.SetFloat("Walk", agent.velocity.sqrMagnitude);
    }

    void OnIdle()
    {
        an.SetFloat("Walk", 0.0f);
    }

    void OnAttack()
    {
        an.SetTrigger("Attack");
    }

    void OnHitted()
    {
        an.SetTrigger("Hitted");
    }

    void Die()
    {
        an.SetTrigger("Die");
    }
}
