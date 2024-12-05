using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDamageNpcState : NpcState
{
    private float damageDuration = 0.5f;
    private float damageStartTime;
    private float invulnerabilityDuration = 2f; 
    //private bool isInvulnerable = false;
    private float knockbackForce = 4f; 
    UnityEngine.AI.NavMeshAgent navMeshAgent;
    Animator animator;
    //Rigidbody rigidbody;
    public GetDamageNpcState(Npc npc, NpcStateMachine stateMachine, UnityEngine.AI.NavMeshAgent navMeshAgent, Animator animator) : base(npc, stateMachine)
    {
        this.navMeshAgent = navMeshAgent;
        this.animator = animator;
        //this.rigidbody = rigidbody;
    }
    public override void EnterState()
    {
        base.EnterState();
        navMeshAgent.isStopped = true;
        damageStartTime = Time.time;
        animator.SetTrigger("Hit");
        npc.GetComponent<Health>().IsInvulnerable = true;
        
        npc.GetInvulnerability(invulnerabilityDuration);
        
        //Knockback();
    }
    public override void Update()
    {
        base.Update();
       
        if (Time.time - damageStartTime >= damageDuration)
        {
            npc.Attack(npc.GetComponent<Health>()?.LastEnemy);
        }
    }
    public override void ExitState()
    {
        base.ExitState();
        navMeshAgent.isStopped = false;
    }

    private void Knockback()
    {
        if (npc.GetComponent<Health>()?.LastEnemy != null)
        {
            Vector3 direction = (npc.transform.position) - (npc.GetComponent<Health>().LastEnemy.transform.position);
            direction.Normalize();
            npc.GetComponent<Rigidbody>().AddForce(direction * knockbackForce, ForceMode.Impulse);
        }
    }
}
