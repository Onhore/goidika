using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NpcDescription))]
[RequireComponent(typeof(Npc))]
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
        if (!GetComponent<Npc>().IsDead())
            Player.instance.Dialogue?.StartDialogue(GetComponent<NpcDescription>());
    }


}
