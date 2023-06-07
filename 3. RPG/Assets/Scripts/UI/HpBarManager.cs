using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarManager : MonoBehaviour
{
    public HpBar hpBar;
    public static HpBarManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void Create(Transform target, CharacterStat stat)
    {
        HpBar newHpBar = Instantiate(hpBar, transform) as HpBar;
        newHpBar.Init(target, stat);
    }
}
