using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    public event Action OnAttack;
    public event Action OnHitted;
    public event Action OnDie;
}
