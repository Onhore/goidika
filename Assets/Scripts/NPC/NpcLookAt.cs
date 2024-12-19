using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcLookAt : MonoBehaviour
{
    public Npc npc;
    public LayerMask lookableLayerMask;
    public float searchRadius = 10f;

    public Vector3 ObjectLookAt
    {
        get
        {
            if (npc.Target != null)
                return npc.Target.transform.position + new Vector3(0, 1.5f, 0);
            else if (npc.FollowTarget != null)
                return npc.FollowTarget.transform.position + new Vector3(0, 1.5f, 0);
            else
                return FindNearestLookableObject();
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
    }

    private Vector3 FindNearestLookableObject()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius, lookableLayerMask);
        if (colliders.Length > 0)
        {
            Collider nearestCollider = null;
            float nearestDistance = Mathf.Infinity;

            foreach (var collider in colliders)
            {
                // Проверка, что найденный объект не является самим NPC
                if (collider.transform != transform.parent.transform)
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestCollider = collider;
                    }
                }
            }

            if (nearestCollider != null)
            {
                return nearestCollider.transform.position + new Vector3(0, 1f, 0);
            }
        }

        // Если не найдено ни одного объекта, возвращаем позицию перед NPC
        return transform.position + transform.forward * searchRadius;
    }
}