using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance = null; 

    void Awake () 
    {
        if (instance == null) 
            instance = this; 
        else if(instance == this)
            Destroy(gameObject); 
    }
    private PlayerDialogue dialogue;
    private PlayerInteract interact;
    public bool onDialogue => dialogue.OnDialogue;
    

    void Start()
    {
        dialogue = GetComponent<PlayerDialogue>();
    }
}
