using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackNpcState : NpcState
{
    private float attackRange = 2f;
    private float attackCooldown = 1f;
    private float damage = 10f;
    private float attackDuration = 0.5f;

    private float lastAttackTime = 0f;
    private bool isAttacking = false;
    private float attackStartTime = 0f;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    public AttackNpcState(Npc npc, NpcStateMachine stateMachine, NavMeshAgent navMeshAgent, Animator animator) : base(npc, stateMachine)
    {
        this.navMeshAgent = navMeshAgent;
        this.animator = animator;
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Attack");
        navMeshAgent.isStopped = false;
    }

    public override void Update()
    {
        base.Update();

        if (npc.Target == null || npc.Target.GetComponent<IDyinable>().IsDead())
        {
            npc.Go();
            return;
        }

        float distanceToTarget = Vector3.Distance(npc.transform.position, npc.Target.transform.position);

        if (distanceToTarget <= attackRange && !isAttacking)
        {
            if (Time.time - lastAttackTime > attackCooldown)
            {
                StartAttack();
                lastAttackTime = Time.time;
            }
        }
        else
        {
            navMeshAgent.SetDestination(npc.Target.transform.position);
        }

        if (isAttacking)
        {
            if (Time.time - attackStartTime > attackDuration)
            {
                FinishAttack();
            }
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        npc.ForgotAttackTarget();
    }

    private void StartAttack()
    {
        isAttacking = true;
        navMeshAgent.isStopped = true;
        animator.SetTrigger("Attack");
        attackStartTime = Time.time;
    }

    private void FinishAttack()
    {
        CheckHitbox();
        isAttacking = false;
        navMeshAgent.isStopped = false;
    }

    private void CheckHitbox()
    {
        Debug.Log("Checking Hitbox");
        if (npc.Target != null)
        {
            Collider[] hitColliders = Physics.OverlapSphere(npc.transform.position + npc.transform.forward * attackRange, attackRange);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject == npc.Target)
                {
                    Debug.Log(npc.Target);
                    hitCollider.gameObject.GetComponent<IDamagable>().Damage(damage, npc.gameObject);
                }
            }
        }
    }
}