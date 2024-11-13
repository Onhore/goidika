using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private LayerMask InteractLayer;
    [SerializeField] private float InteractRange = 1;
    private bool onDialogue => Player.instance.onDialogue;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !onDialogue)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, InteractRange, InteractLayer))
            {
                hit.collider.gameObject.GetComponent<IInteractable>()?.Interact();
            }
        }

    }
}
