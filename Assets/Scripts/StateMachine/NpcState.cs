using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NpcState
{
    protected Npc npc;
    protected NpcStateMachine stateMachine;

    public NpcState(Npc npc, NpcStateMachine stateMachine)
    {
        this.npc = npc;
        this.stateMachine = stateMachine;
    }

    public virtual void EnterState()
    {

    }
    public virtual void ExitState()
    {

    }
    public virtual void Update()
    {
        
    }
}
