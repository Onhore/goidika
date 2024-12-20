using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DialogueNpcState : NpcState
{
    private Npc npc;
    private NavMeshAgent navMeshAgent;
    private float dialogueRotationSpeed;
    private GameObject target => npc.Target;
    public DialogueNpcState(Npc npc, NpcStateMachine stateMachine, NavMeshAgent navMeshAgent, float dialogueRotationSpeed) : base(npc, stateMachine)
    {
        this.npc = npc;
        this.navMeshAgent = navMeshAgent;
        this.dialogueRotationSpeed = dialogueRotationSpeed;
    }
    public override void EnterState()
    {
        base.EnterState();
        navMeshAgent.isStopped = true;
        npc.OnDialogue = true;
        if (target.GetComponent<Npc>() != null && !target.GetComponent<Npc>().OnDialogue)
            ChatGPTManager.instance.BroadcastMessageWithReaction("Диалог "+ target.name +" и " + npc.name + " начинается.", new GameObject[] {npc.gameObject, target});
        else if (target.GetComponent<Npc>() == null)
            ChatGPTManager.instance.BroadcastMessageWithReaction("Диалог "+ target.name +" и " + npc.name + " начинается.", new GameObject[] {npc.gameObject, target});
    }
    public override void Update()
    {
        base.Update();
        Vector3 direction = target.transform.position - npc.transform.position;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            npc.transform.rotation = Quaternion.Lerp(npc.transform.rotation, targetRotation, dialogueRotationSpeed * Time.deltaTime);
        }
    }
    public override void ExitState()
    {
        if (target.GetComponent<Npc>() != null && target.GetComponent<Npc>().OnDialogue)
            ChatGPTManager.instance.BroadcastMessageWithReaction("Диалог "+ target.name +" и " + npc.name + " заканчивается.", new GameObject[] {npc.gameObject, target});
        else if (target.GetComponent<Npc>() == null)
            ChatGPTManager.instance.BroadcastMessageWithReaction("Диалог "+ target.name +" и " + npc.name + " заканчивается.", new GameObject[] {npc.gameObject, target});
        npc.OnDialogue = false;
        base.ExitState();
        //npc.EndDialogueEvent.
       //npc.EndDialogueEvent.
    }
}
