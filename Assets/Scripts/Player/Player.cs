using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance = null; 

    private PlayerDialogue dialogue;
    public PlayerDialogue Dialogue => dialogue;
    private PlayerInteract interact;
    public PlayerInteract Interact => interact;
    private PlayerInventory inventory;
    public PlayerInventory Inventory => inventory;

    public bool HandsFull => inventory.HandsFull;
    public bool onDialogue => dialogue.OnDialogue;
    

    void Awake()
    {
        if (instance == null) 
            instance = this; 
        else if(instance == this)
            Destroy(gameObject); 

        dialogue = GetComponent<PlayerDialogue>();
        interact = GetComponent<PlayerInteract>();
        inventory = GetComponent<PlayerInventory>();
    }
    
}
