using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowNpcState : NpcState
{
    private NavMeshAgent navMeshAgent;
    private GameObject followTarget => npc.FollowTarget;
    //private Transform ComebackPoint;
    private GoToNpcState goToNpcState;
    private IdleNpcState idleNpcState;
    private float maxFollowDistance;
    private float followDistance;
    private Animator animator;
    public FollowNpcState(Npc npc, NpcStateMachine stateMachine, NavMeshAgent navMeshAgent, GoToNpcState goToNpcState, IdleNpcState idleNpcState, float maxFollowDistance, float followDistance, Animator animator) : base(npc, stateMachine)
    {
        this.navMeshAgent = navMeshAgent;
        this.goToNpcState = goToNpcState;
        this.maxFollowDistance = maxFollowDistance;
        this.followDistance = followDistance;
        this.animator = animator;
        this.idleNpcState = idleNpcState;
    }

    public override void EnterState()
    {
        base.EnterState();
    }
    public override void Update()
    {
        base.Update();
        if (followTarget == null)
            npc.Idle();

        //Логика следования
        float distanceToTarget = Vector3.Distance(npc.transform.position, followTarget.transform.position);
        if (distanceToTarget > maxFollowDistance)
        {
            npc.Go();
        }
        else if (distanceToTarget > followDistance)
        {
            navMeshAgent.destination = followTarget.transform.position;
            animator.SetBool("isWalking", true);
            Debug.Log("Следование");
            navMeshAgent.isStopped = false;
        }
        else
        {
            navMeshAgent.isStopped = true;
            animator.SetBool("isWalking", false);
        }

    }
    public override void ExitState()
    {
        base.ExitState();

    }
}
