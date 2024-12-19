using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcDialogueNpcState : NpcState
{
    private Npc npc;
    private NavMeshAgent navMeshAgent;
    public NpcDialogueNpcState(Npc npc, NpcStateMachine stateMachine, NavMeshAgent navMeshAgent) : base(npc, stateMachine)
    {
        this.npc = npc;
        this.navMeshAgent = navMeshAgent;
    }
}
