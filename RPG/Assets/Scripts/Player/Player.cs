using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    public CharacterStat stat;
    public CharacterCombat combat;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        stat.OnHpzero += Die;
    }

    private void OnDisable()
    {
        stat.OnHpzero -= Die;
    }

    void Die()
    {

    }
}
