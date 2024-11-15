using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class Npc : MonoBehaviour
{
    public enum StateMachine
    {
        Idle,
        GoTo,
        Follow,
        Attack,
        Death,
        Dialogue
    }
    public StateMachine currentState;
    public StateMachine lastState;

    private NavMeshAgent navMeshAgent;
    public Transform moveTrans;
    private GameObject followTarget;
    private GlobalLists.Place goToPlace;
    private bool inDialogue = false;

    public event System.Action EndDialogueEvent;
    [SerializeField] private float dialogueRotationSpeed;

    private NpcDescription npcDescription;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        npcDescription = GetComponent<NpcDescription>();
    }

    private void Start()
    {
        currentState = StateMachine.Idle;
        Debug.Log(navMeshAgent.destination);
    }

    private void Update()
    {
        switch (currentState)
        {
            case StateMachine.Idle:
                IdleUpdate();
                break;
            case StateMachine.GoTo:
                GoToUpdate();
                break;
            case StateMachine.Follow:
                FollowUpdate();
                break;
            case StateMachine.Attack:
                AttackUpdate();
                break;
            case StateMachine.Dialogue:
                DialogueUpdate();
                break;
        }
    }

    private void IdleUpdate()
    {
        // Implement idle behavior
    }

    private void GoToUpdate()
    {
        if (navMeshAgent.isStopped)
            navMeshAgent.isStopped = false;
         if (Vector3.Distance(transform.position, navMeshAgent.destination) <= navMeshAgent.stoppingDistance)
        {
            navMeshAgent.isStopped = true;
            npcDescription.AddSystemMessage("Вы прибыли в место: " + goToPlace.Name + ". Описание: " + goToPlace.Description);
            Debug.Log("Reached destination, switching to Idle state");
            goToPlace = null;
            currentState = StateMachine.Idle;
        }
    }

    private void FollowUpdate()
    {
        if (followTarget != null)
        {
            navMeshAgent.destination = followTarget.transform.position;
        }
        else
        {
            currentState = StateMachine.Idle;
        }
    }

    private void AttackUpdate()
    {
        // Implement attack behavior
    }

    private void DialogueUpdate()
    {


            Vector3 direction = Player.instance.transform.position - transform.position;
            direction.y = 0; // Игнорируем вертикальное смещение
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Проверяем, стоит ли NPC к игроку спиной
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, dialogueRotationSpeed * Time.deltaTime);

        
    }

    public void Idle()
    {
        currentState = StateMachine.Idle;
    }

    public void Go(GlobalLists.Place place)
    {
        goToPlace = place;
        currentState = StateMachine.GoTo;
        navMeshAgent.destination = goToPlace.Coordinates.position;
    }
    public void StopGo()
    {
        goToPlace = null;
        //currentState = StateMachine.Idle;
        lastState = StateMachine.Idle;
        navMeshAgent.destination = transform.position;
    }
    public void Follow(GameObject target)
    {
        followTarget = target;
        currentState = StateMachine.Follow;
    }
    public void StopFollow()
    {
        followTarget = null;
        lastState = StateMachine.Idle;
    }

    public void Attack(LayerMask enemyLayerMask)
    {
        currentState = StateMachine.Attack;
    }

    public void StartDialogue()
    {
        if (currentState == StateMachine.GoTo)
            navMeshAgent.isStopped = true;
        inDialogue = true;
        lastState = currentState;
        currentState = StateMachine.Dialogue;
    }

    public void EndDialogue()
    {
        inDialogue = false;
        if (lastState == StateMachine.GoTo)
            navMeshAgent.isStopped = false;
        currentState = lastState;
        EndDialogueEvent?.Invoke();
    }
}