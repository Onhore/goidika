using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomWalkNpcState : NpcState
{
    private NavMeshAgent navMeshAgent;
    private float wanderRadius = 5f; // Радиус, в пределах которого NPC будет бродить
    private float minWanderTimer = 30f; // Минимальное время, через которое NPC выберет новую точку
    private float maxWanderTimer = 60f; // Максимальное время, через которое NPC выберет новую точку
    private float timer;
    private float waitTimer = 40f; // Время ожидания на точке
    private float maxStuckTimer = 10f; // Максимальное время, которое NPC может быть застрявшим
    private Vector3 randomPoint;
    private Vector3 initialPosition;
    private bool isWaiting;
    private float stuckTimer;

    public RandomWalkNpcState(Npc npc, NpcStateMachine stateMachine, NavMeshAgent navMeshAgent) : base(npc, stateMachine)
    {
        this.navMeshAgent = navMeshAgent;
    }

    public override void EnterState()
    {
        base.EnterState();
        navMeshAgent.isStopped = false;
        initialPosition = npc.transform.position; // Сохраняем начальное положение NPC
        GetNewRandomPoint();
    }

    public override void Update()
    {
        base.Update();

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                isWaiting = false;
                GetNewRandomPoint();
            }
        }
        else
        {
            timer -= Time.deltaTime;
            stuckTimer -= Time.deltaTime;

            if (timer <= 0)
            {
                GetNewRandomPoint();
            }

            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
            {
                isWaiting = true;
                waitTimer = 40f; // Устанавливаем таймер ожидания на 40 секунд
                navMeshAgent.isStopped = true; // Отключаем движение
                stuckTimer = maxStuckTimer; // Сброс таймера застревания
                Debug.Log("NPC reached destination and is now waiting.");
            }
            else if (stuckTimer <= 0)
            {
                // Если NPC застрял, выбираем новую точку
                GetNewRandomPoint();
                stuckTimer = maxStuckTimer; // Сброс таймера застревания
            }
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        navMeshAgent.isStopped = true;
    }

    private void GetNewRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += initialPosition; // Используем начальное положение NPC
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1))
        {
            randomPoint = hit.position;
            navMeshAgent.destination = randomPoint;
            navMeshAgent.isStopped = false; // Включаем движение
            timer = Random.Range(minWanderTimer, maxWanderTimer); // Рандомизируем таймер
            stuckTimer = maxStuckTimer; // Сброс таймера застревания
            Debug.Log("NPC is moving to a new random point.");
        }
    }
}