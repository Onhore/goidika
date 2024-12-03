using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IdleNpcState : NpcState
{
    public IdleNpcState(Npc npc, NpcStateMachine stateMachine) : base(npc, stateMachine)
    {
    }
    public override void EnterState()
    {
        base.EnterState();
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
