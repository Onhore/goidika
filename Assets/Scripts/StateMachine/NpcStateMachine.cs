using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStateMachine
{
    public NpcState CurrentState { get; set; }
    public NpcState LastNpcState { get; set; }
    public void Init(NpcState startingState)
    {
        CurrentState = startingState;
        CurrentState.EnterState();
    }
    public void ChangeState(NpcState newState)
    {
        CurrentState.ExitState();
        LastNpcState = CurrentState;
        CurrentState = newState;
        CurrentState.EnterState();
    }
}
