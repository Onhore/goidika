using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public LayerMask InteractLayer;
    public float InteractRange = 1;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, InteractLayer))
            {
                hit.collider.gameObject.GetComponent<IInteractable>()?.Interact();
                //GetComponent<QMovement>().enabled = false;
            }
        }
    }
}
