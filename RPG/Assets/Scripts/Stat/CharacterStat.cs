using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    public int maxHp;
    int currentHp;
    public int CurrentHp
    {
        get => currentHp;
        set => currentHp = value;
    }

    public int power = 10;

    public event Action OnHpzero;

    private void OnEnable()
    {
        currentHp = maxHp;
    }

    public void Heal(int heal)
    {
        currentHp += heal;

        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
    }

    public void Hitted(int damage)
    {
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHp -= damage;

        if (currentHp < 0)
        {
            OnHpzero?.Invoke();
        }
    }
}
