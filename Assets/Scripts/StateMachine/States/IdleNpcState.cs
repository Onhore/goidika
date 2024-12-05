using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class IdleNpcState : NpcState
{
    NavMeshAgent navMeshAgent;
    public IdleNpcState(Npc npc, NpcStateMachine stateMachine, NavMeshAgent navMeshAgent) : base(npc, stateMachine)
    {
        this.navMeshAgent = navMeshAgent;
    }
    public override void EnterState()
    {
        base.EnterState();
        navMeshAgent.isStopped = true;
    }
    public override void Update()
    {
        base.Update();
    }
    public override void ExitState()
    {
        base.ExitState();
    }
}
