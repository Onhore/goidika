using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomWalkNpcState : NpcState
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private float wanderRadius = 10f; // Радиус, в пределах которого NPC будет бродить
    private float minWanderTimer = 2f; // Минимальное время, через которое NPC выберет новую точку
    private float maxWanderTimer = 5f; // Максимальное время, через которое NPC выберет новую точку
    private float timer;
    private Vector3 randomPoint;

    public RandomWalkNpcState(Npc npc, NpcStateMachine stateMachine, NavMeshAgent navMeshAgent, Animator animator) : base(npc, stateMachine)
    {
        this.navMeshAgent = navMeshAgent;
        this.animator = animator;
    }

    public override void EnterState()
    {
        base.EnterState();
        animator.SetBool("isWalking", true);
        GetNewRandomPoint();
    }

    public override void Update()
    {
        base.Update();

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            GetNewRandomPoint();
        }

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
        {
            GetNewRandomPoint();
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        animator.SetBool("isWalking", false);
    }

    private void GetNewRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += npc.transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1))
        {
            randomPoint = hit.position;
            navMeshAgent.destination = randomPoint;
            timer = Random.Range(minWanderTimer, maxWanderTimer); // Рандомизируем таймер
        }
    }
}