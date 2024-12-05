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
        if (GetComponent<Npc>().IsDead())
            return;
        
        if (Player.instance.HandsFull && Player.instance.pickedItem is not Weapon)
        {
            Player.instance.pickedItem.GiveToNpc(GetComponent<Npc>());
        }
        else
        {
            //Debug.Log("Interact!");
            Player.instance.Dialogue?.StartDialogue(GetComponent<NpcDescription>());
        }
    }


}
