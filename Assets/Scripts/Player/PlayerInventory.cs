using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    //private List<Item> inventory;
    private Item ActiveSlot;
    public Item Item => ActiveSlot;
    public bool HandsFull => ActiveSlot != null;
    private bool onDialogue => Player.instance.onDialogue;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && HandsFull && !onDialogue)
        {
            DropItem();
        }
    }
    public void PickUpItem(Item item)
    {
        if (HandsFull) return; 
        ActiveSlot = item;
        ActiveSlot.PickUp();
    }
    public void DropItem()
    {
        if (!HandsFull) return; 
        ActiveSlot.Drop();
        ActiveSlot = null;
    }
}
