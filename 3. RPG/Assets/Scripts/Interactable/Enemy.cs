using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Interactable
{
    CharacterStat stat;

    private void OnEnable()
    {
        stat = GetComponent<CharacterStat>();
        stat.OnHpzero += Die;
    }

    private void OnDisable()
    {
        stat.OnHpzero -= Die;
    }

    public override void Interact()
    {
        Player.instance.combat.Attack(stat);
    }

    void Die()
    {
        PoolingManager.instance.ReturnObject(this.gameObject);
    }
}
