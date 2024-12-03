using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NpcDescription))]
[RequireComponent(typeof(NpcInteract))]
public class Npc : MonoBehaviour
{
    NpcStateMachine stateMachine;
    private IdleNpcState IdleState;
    private DialogueNpcState DialogueState;
    private FollowNpcState  FollowState;
    private GoToNpcState  GoToState;
    private NavMeshAgent navMeshAgent;
    //public Transform GoToPoint;
    public GameObject FollowTarget {private set; get;}
    public GlobalLists.Place GoToPlace {private set; get;}
    public Vector3 DefaultBasePoint {private set; get;}
    private bool inDialogue => stateMachine.CurrentState is DialogueNpcState;

    public event System.Action EndDialogueEvent;
    [SerializeField] private float dialogueRotationSpeed;
    [SerializeField] private float followDistance = 5f; 
    [SerializeField] private float maxFollowDistance = 20f; 
    [SerializeField] private float placeDistance = 3f;


    private NpcDescription npcDescription;
    public Animator animator;

    private void Awake()
    {
        DefaultBasePoint = transform.position;
        navMeshAgent = GetComponent<NavMeshAgent>();
        npcDescription = GetComponent<NpcDescription>();
        stateMachine = new NpcStateMachine();
        //animator = GetComponent<Animator>();
        IdleState = new IdleNpcState(this, stateMachine);
        DialogueState = new DialogueNpcState(this, stateMachine, navMeshAgent, dialogueRotationSpeed);
        FollowState = new FollowNpcState(this, stateMachine, navMeshAgent, GoToState, IdleState, maxFollowDistance,followDistance,animator);
        GoToState = new GoToNpcState(this, stateMachine, navMeshAgent, IdleState, animator, placeDistance, DefaultBasePoint);
    }

    private void Start()
    {
        //currentState = StateMachine.Idle;
        //Debug.Log(navMeshAgent.destination);
        stateMachine.Init(IdleState);
    }

    private void Update()
    {
        stateMachine.CurrentState.Update();
        /*switch (currentState)
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
        }*/
    }

    private void IdleUpdate()
    {
        // Implement idle behavior
        //animator.SetBool("isWalking", false);
    }

    /*private void GoToUpdate()
    {
        if (navMeshAgent.isStopped)
            navMeshAgent.isStopped = false;
        if (Vector3.Distance(transform.position, navMeshAgent.destination) <= PlaceDistance)
        {
            navMeshAgent.isStopped = true;
            //npcDescription.AddSystemMessage("Вы прибыли в место: " + goToPlace.Name + ". Описание: " + goToPlace.Description);
            Debug.Log("Reached destination, switching to Idle state");
            goToPlace = null;
            //currentState = StateMachine.Idle;
        }
        animator.SetBool("isWalking", true);
    }*/

    /*private void FollowUpdate()
    {
        if (followTarget != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, followTarget.transform.position);
            if (distanceToTarget > maxFollowDistance)
            {
                navMeshAgent.destination = defaultBasePoint.position;
                //currentState = StateMachine.GoTo;
                followTarget = null;
                Debug.Log("Target is too far, returning to base point");
            }
            else if (distanceToTarget > followDistance)
            {
                navMeshAgent.destination = followTarget.transform.position;
                animator.SetBool("isWalking", true);
                Debug.Log("Следование");
                navMeshAgent.isStopped = false;
            }
            else
            {
                navMeshAgent.isStopped = true;
                animator.SetBool("isWalking", false);
            }
        }
        else
        {
            //currentState = StateMachine.Idle;
        }
    }*/

    private void AttackUpdate()
    {
        // Implement attack behavior
    }

    /*private void DialogueUpdate()
    {
        Vector3 direction = Player.instance.transform.position - transform.position;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, dialogueRotationSpeed * Time.deltaTime);
        }
    }*/

    public void Idle()
    {
        stateMachine.ChangeState(IdleState);
    }

    public void Go(GlobalLists.Place place)
    {
        GoToPlace = place;
        stateMachine.ChangeState(GoToState);
        //currentState = StateMachine.GoTo;
        //navMeshAgent.destination = GoToPlace.Coordinates.position;
    }
    public void Go()
    {
        GoToPlace = null;
        stateMachine.ChangeState(GoToState);
        //currentState = StateMachine.GoTo;
        //navMeshAgent.destination = GoToPlace.Coordinates.position;
    }

    public void StopGo()
    {
        GoToPlace = null;
        stateMachine.ChangeState(IdleState);
        //lastState = StateMachine.Idle;
        //navMeshAgent.destination = transform.position;
    }

    public void Follow(GameObject target)
    {
        FollowTarget = target;
        stateMachine.ChangeState(FollowState);
    }

    public void StopFollow()
    {
        EndDialogueEvent = null;
        Debug.Log("Стоп следование");
        FollowTarget = null;
        stateMachine.ChangeState(IdleState);
    }

    public void Attack(LayerMask enemyLayerMask)
    {
        //currentState = StateMachine.Attack;
    }

    public void StartDialogue()
    {
        //if (//currentState == StateMachine.GoTo)
        //navMeshAgent.isStopped = true;
        stateMachine.ChangeState(DialogueState);
        //inDialogue = true;
        //lastState = currentState;
        //currentState = StateMachine.Dialogue;
    }

    public void EndDialogue()
    {
        //inDialogue = false;
        //if (//lastState == StateMachine.GoTo)
        //navMeshAgent.isStopped = false;
        //currentState = lastState;
        stateMachine.ChangeState(IdleState);
        EndDialogueEvent?.Invoke();
        
    }
}