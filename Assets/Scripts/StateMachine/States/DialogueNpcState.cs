using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DialogueNpcState : NpcState
{
    private NavMeshAgent navMeshAgent;
    private float dialogueRotationSpeed;
    public DialogueNpcState(Npc npc, NpcStateMachine stateMachine, NavMeshAgent navMeshAgent, float dialogueRotationSpeed) : base(npc, stateMachine)
    {
        this.navMeshAgent = navMeshAgent;
        this.dialogueRotationSpeed = dialogueRotationSpeed;
    }
    public override void EnterState()
    {
        base.EnterState();
        navMeshAgent.isStopped = true;
    }
    public override void Update()
    {
        base.Update();
        Vector3 direction = Player.instance.transform.position - npc.transform.position;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            npc.transform.rotation = Quaternion.Lerp(npc.transform.rotation, targetRotation, dialogueRotationSpeed * Time.deltaTime);
        }
    }
    public override void ExitState()
    {
        base.ExitState();
        //npc.EndDialogueEvent.
       //npc.EndDialogueEvent.
    }
}
