using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NpcDescription))]
public class NpcInteract : MonoBehaviour, IInteractable
{
    public PlayerDialogue UIDialogue;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        Debug.Log("Interact!");
        Player.instance.Dialogue?.StartDialogue(GetComponent<NpcDescription>());
    }


}
