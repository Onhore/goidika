using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public enum StateMachine
    {
        Idle,
        GoTo,
        Attack,
        Death
    }
    public StateMachine currentState;

    private void Start()
    {
        currentState = StateMachine.Idle;
    }
    private void Update()
    {
        switch(currentState)
        {
            case StateMachine.Idle:
                Idle();
                break;
            case StateMachine.GoTo:
                GoTo();
                break;
            case StateMachine.Attack:
                Attack();
                break;
        }
    }

    private void Idle()
    {

    }
    private void GoTo()
    {

    }
    private void Attack()
    {

    }
}
