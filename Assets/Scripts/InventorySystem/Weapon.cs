using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    [SerializeField] private int damage;
    public Animator animator; 
    public LayerMask hitTargetLayer; 
    public float attackRange = 2.0f; 
    

    public int Damage => damage;

    public void Attack()
    {
        animator.Play("Attack");

        // Создать сферу в направлении взгляда игрока
        Vector3 attackPosition = transform.position + transform.forward * attackRange;
        Collider[] hitTargets = Physics.OverlapSphere(attackPosition, attackRange, hitTargetLayer);
        
        // Нанести урон всем объектам внутри сферы
        foreach (Collider target in hitTargets)
        {
            // Предполагается, что у целей есть компонент, который может получать урон
            IDamagable damageable = target.GetComponent<IDamagable>();
            if (damageable != null)
            {
                //Debug.Log("Получение урона");
                damageable.Damage(damage, pickedUpBy);
            }
    }
    }
}
