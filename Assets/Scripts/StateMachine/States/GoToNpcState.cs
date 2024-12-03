using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class GoToNpcState : NpcState
{
    private NavMeshAgent navMeshAgent;
    private GameObject followTarget => npc.FollowTarget;
    
    private IdleNpcState idleState;
    private Animator animator;
    private float placeDistance;
    private GlobalLists.Place goToPlace => npc?.GoToPlace;
    private Vector3 defaultBasePoint;
    public GoToNpcState(Npc npc, NpcStateMachine stateMachine, NavMeshAgent navMeshAgent, IdleNpcState idleState, Animator animator, float placeDistance, Vector3 defaultPoint) : base(npc, stateMachine)
    {
        this.idleState = idleState;
        this.animator = animator;
        this.placeDistance = placeDistance;
        this.navMeshAgent = navMeshAgent;
        this.defaultBasePoint = defaultPoint;
    }
    public override void EnterState()
    {
        base.EnterState();
        animator.SetBool("isWalking", true);
        
        if(goToPlace == null)
            navMeshAgent.destination = defaultBasePoint;
        else
            navMeshAgent.destination = goToPlace.Coordinates.position;
    }
    public override void Update()
    {
        base.Update();
        if (navMeshAgent.isStopped)
            navMeshAgent.isStopped = false;
        if (Vector3.Distance(npc.transform.position, navMeshAgent.destination) <= placeDistance)
        {
            navMeshAgent.isStopped = true;
            //npcDescription.AddSystemMessage("Вы прибыли в место: " + goToPlace.Name + ". Описание: " + goToPlace.Description);
            //Debug.Log("Reached destination, switching to Idle state");
            npc.Idle();
            //currentState = StateMachine.Idle;
        }
        
    }
    public override void ExitState()
    {
        base.ExitState();
        animator.SetBool("isWalking", false);
    }
}
