using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NpcLookAt : MonoBehaviour
{
    public Npc npc;
    public Vector3 ObjectLookAt 
    {
    get
    {
        if (npc.Target!=null)
            return npc.Target.transform.position + new Vector3(0,1.5f,0);
        else if (npc.FollowTarget!=null)
            return npc.FollowTarget.transform.position  + new Vector3(0,1.5f,0);
        else
            return Camera.main.transform.position;
    }
    }
    void Awake()
    {
        npc = GetComponentInParent<Npc>();
    }
    public Animator animator;
    public float headWeight;
    public float bodyWeight;
    
    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetLookAtPosition(ObjectLookAt);
        animator.SetLookAtWeight(1, bodyWeight, headWeight);
        //Debug.Log(2);
    }
}
