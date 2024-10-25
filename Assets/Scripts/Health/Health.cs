using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : Progressive, IDamagable, IHealable
{
    [SerializeField] private UnityEvent OnDie;

    public void Damage(float amount)
    {
        Current -= amount;
        if (Current <= 0f)
            OnDie.Invoke();
    }
    public void Heal(float amount)
    {
        Current += amount;
        if (Current > Initial)
            Current = Initial;
    }
}
