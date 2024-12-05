using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : Progressive, IDamagable, IHealable
{
    public bool IsInvulnerable { get;set;}
    [SerializeField] private UnityEvent OnDamage;
    [SerializeField] private UnityEvent OnDie;
    public GameObject LastEnemy {private set; get;}

    public void Damage(float amount, GameObject enemy)
    {
        //Debug.Log(IsInvulnerable);
        if (IsInvulnerable)
            return;
        Current -= amount;
        LastEnemy = enemy;
        if (Current <= 0f)
            OnDie.Invoke();
        else
            OnDamage.Invoke();
        
    
    }
    public void Heal(float amount)
    {
        Current += amount;
        if (Current > Initial)
            Current = Initial;
    }
}
