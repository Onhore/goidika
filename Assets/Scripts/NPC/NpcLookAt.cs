using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NpcLookAt : MonoBehaviour
{
    public Transform objectLookAt;
    public Animator animator;
    public float headWeight;
    public float bodyWeight;
    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetLookAtPosition(objectLookAt.position);
        animator.SetLookAtWeight(1, bodyWeight, headWeight);
        //Debug.Log(2);
    }
}
