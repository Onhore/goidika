using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ApproachNpcState : NpcState
{
    private NavMeshAgent navMeshAgent;
    private GameObject target => npc.FollowTarget;
    private float approachDistance;
    //private Animator animator;

    public ApproachNpcState(Npc npc, NpcStateMachine stateMachine, NavMeshAgent navMeshAgent, float approachDistance) : base(npc, stateMachine)
    {
        this.navMeshAgent = navMeshAgent;
        this.approachDistance = approachDistance;
        //this.animator = animator;
    }

    public override void EnterState()
    {
        base.EnterState();
        navMeshAgent.destination = target.transform.position;
        navMeshAgent.isStopped = false;
        //animator.SetBool("isWalking", true);
    }

    public override void Update()
    {
        base.Update();

        if (target == null)
        {
            npc.Idle();
            return;
        }

        float distanceToTarget = Vector3.Distance(npc.transform.position, target.transform.position);

        if (distanceToTarget <= approachDistance)
        {
            navMeshAgent.isStopped = true;
            //animator.SetBool("isWalking", false);
            target.GetComponent<NpcDescription>()?.ReactSystemMessage("К вам подошел " + npc.name);
            npc.GetComponent<NpcDescription>()?.ReactSystemMessage("Вы подошли к цели.");
            npc.Idle(); // Переход в состояние простоя или другое состояние
        }
        else
        {
            navMeshAgent.destination = target.transform.position;
            //animator.SetBool("isWalking", true);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        navMeshAgent.isStopped = true;
        //animator.SetBool("isWalking", false);
    }
}