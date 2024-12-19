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
        npc.OnDialogue = true;
        ChatGPTManager.instance.BroadcastMessageWithReaction("Player начинает диалог с " + npc.name , new GameObject[] {npc.gameObject, npc.Target});
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
        ChatGPTManager.instance.BroadcastMessageWithReaction("Диалог Player и " + npc.name + " заканчивается.", new GameObject[] {npc.gameObject, Player.instance.gameObject});
        npc.OnDialogue = false;
        base.ExitState();
        //npc.EndDialogueEvent.
       //npc.EndDialogueEvent.
    }
}
