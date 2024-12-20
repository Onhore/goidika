using System.Collections;
using System.Collections.Generic;
using com.cyborgAssets.inspectorButtonPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NpcDescription))]
[RequireComponent(typeof(NpcInteract))]
public class Npc : MonoBehaviour, IDyinable
{
    NpcStateMachine stateMachine;
    private IdleNpcState IdleState;
    private DialogueNpcState DialogueState;
    private FollowNpcState  FollowState;
    private GoToNpcState  GoToState;
    private AttackNpcState  AttackState;
    private GetDamageNpcState  GetDamageState;
    private RandomWalkNpcState RandomWalkState;
    private ApproachNpcState ApproachState;
    private DeadNpcState  DeadState;
    private NavMeshAgent navMeshAgent;
    //public Transform GoToPoint;
    public GameObject Target {private set; get;}
    public GameObject FollowTarget {private set; get;}
    public GlobalLists.Place GoToPlace {private set; get;}
    public Vector3 DefaultBasePoint {private set; get;}
    private bool inDialogue => stateMachine.CurrentState is DialogueNpcState;

    public event System.Action EndDialogueEvent;
    [SerializeField] private float dialogueRotationSpeed;
    [SerializeField] private float followDistance = 5f; 
    [SerializeField] private float maxFollowDistance = 20f; 
    [SerializeField] private float placeDistance = 3f;
    [SerializeField] private float deadTime = 30f;
    [SerializeField] private float AttackDamage;
    [SerializeField] private LayerMask Hittable;
    public bool OnDialogue = false;
    public bool isDead = false;

    private NpcDescription npcDescription;
    public Animator animator;

    private void Awake()
    {
        DefaultBasePoint = transform.position;
        navMeshAgent = GetComponent<NavMeshAgent>();
        npcDescription = GetComponent<NpcDescription>();
        stateMachine = new NpcStateMachine();
        //animator = GetComponent<Animator>();
        IdleState = new IdleNpcState(this, stateMachine, navMeshAgent);
        DialogueState = new DialogueNpcState(this, stateMachine, navMeshAgent, dialogueRotationSpeed);
        FollowState = new FollowNpcState(this, stateMachine, navMeshAgent, GoToState, IdleState, maxFollowDistance,followDistance,animator);
        GoToState = new GoToNpcState(this, stateMachine, navMeshAgent, IdleState, animator, placeDistance, DefaultBasePoint);
        RandomWalkState = new RandomWalkNpcState(this, stateMachine, navMeshAgent);
        ApproachState = new ApproachNpcState(this, stateMachine, navMeshAgent, 1.5f);
        AttackState = new AttackNpcState(this, stateMachine, navMeshAgent, animator, AttackDamage);
        GetDamageState = new GetDamageNpcState(this, stateMachine, navMeshAgent, animator);
        DeadState = new DeadNpcState(this, stateMachine, navMeshAgent, animator, GetComponent<CharacterController>(), deadTime);
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
        animator.SetBool("isWalking", !navMeshAgent.isStopped);
        animator.SetBool("Dead", isDead);
    }

    private void IdleUpdate()
    { 
        // Implement idle behavior
        //animator.SetBool("isWalking", false);
    }

    private void AttackUpdate()
    {
        // Implement attack behavior
    }
    [ProButton]
    public void Idle()
    {
        stateMachine.ChangeState(IdleState);
    }
   // [ProButton]
    public void Go(GlobalLists.Place place)
    {
        GoToPlace = place;
        stateMachine.ChangeState(GoToState);
        //currentState = StateMachine.GoTo;
        //navMeshAgent.destination = GoToPlace.Coordinates.position;
    }
    [ProButton]
    public void Go()
    {
        GoToPlace = null;
        stateMachine.ChangeState(GoToState);
        //currentState = StateMachine.GoTo;
        //navMeshAgent.destination = GoToPlace.Coordinates.position;
    }
    [ProButton]
    public void StopGo()
    {
        GoToPlace = null;
        stateMachine.ChangeState(IdleState);
        //lastState = StateMachine.Idle;
        //navMeshAgent.destination = transform.position;
    }
    [ProButton]
    public void Follow(GameObject target)
    {
        FollowTarget = target;
        stateMachine.ChangeState(FollowState);
    }
    [ProButton]
    public void StopFollow()
    {
        EndDialogueEvent = null;
        //Debug.Log("Стоп следование");
        FollowTarget = null;
        stateMachine.ChangeState(IdleState);
    }
    [ProButton]
    public void Attack(GameObject target)
    {
        if(target == null)
            Idle();
        if (Extensions.UnityAdditions.IsObjectInLayerMask(target, Hittable))
        {
            Target = target;
            stateMachine.ChangeState(AttackState);
            //Debug.Log("Attack Start");
        }   
        //currentState = StateMachine.Attack;
    }
    [ProButton]
    public void GetDamage()
    {
        stateMachine.ChangeState(GetDamageState);
    }
    [ProButton]
    public void RandomWalk()
    {
        stateMachine.ChangeState(RandomWalkState);
    }
    [ProButton]
    public void ApproachTo(GameObject target)
    {
        FollowTarget = target;
        stateMachine.ChangeState(ApproachState);
    }
    public void StartDialogue(GameObject target)
    {
        //if (//currentState == StateMachine.GoTo)
        //navMeshAgent.isStopped = true;
        Target = target;
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
        EndDialogueEvent = null;
        
    }
    public void GetInvulnerability(float duration)
    {
        StartCoroutine(InvulnerabilityEffect(duration));
    }
    private IEnumerator InvulnerabilityEffect(float duration)
    {
        yield return new WaitForSeconds(duration);
        GetComponent<Health>().IsInvulnerable = false;
    }
    [ProButton]
    public void Death()
    {
        Target = null;
        FollowTarget = null;
        GetDamageState.ClearFlag();
        stateMachine.ChangeState(DeadState);
    }
    public void ForgotAttackTarget()
    {
        Target = null;
    }
    public bool IsDead() => isDead;
}

namespace Extensions
{
    public static class UnityAdditions
    {
    public static bool IsObjectInLayerMask(GameObject obj, LayerMask layerMask)
    {
        int objLayer = obj.layer;
        int mask = layerMask.value;
        return (mask & (1 << objLayer)) > 0;
    }
    }
    
}