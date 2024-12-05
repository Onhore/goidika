using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class DeadNpcState : NpcState
{
    NavMeshAgent navMeshAgent;
    Animator animator;
    CharacterController collider;
    private float timerDuration = 5f; 
    private float timer = 0f;
    public DeadNpcState(Npc npc, NpcStateMachine stateMachine, NavMeshAgent navMeshAgent, Animator animator, CharacterController Coll, float time) : base(npc, stateMachine)
    {
        this.navMeshAgent = navMeshAgent;
        this.animator = animator;
        collider = Coll;
        timerDuration = time;
    }
    public override void EnterState()
    {
        base.EnterState();
        navMeshAgent.isStopped = true;
        animator.SetTrigger("Die");
        npc.GetComponent<Health>().IsInvulnerable = true;
        npc.isDead=true;
        collider.enabled = false;
    }
    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (timer >= timerDuration)
        {
            npc.Idle();
            timer = 0f;
        }
    }
    public override void ExitState()
    {
        base.ExitState();
        npc.isDead=false;
        npc.GetComponent<Health>().IsInvulnerable = false;
        npc.GetComponent<IHealable>().Heal(30);
        collider.enabled = true;
    }
}
