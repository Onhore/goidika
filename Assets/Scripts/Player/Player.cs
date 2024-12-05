using System.Collections;
using System.Collections.Generic;
using com.cyborgAssets.inspectorButtonPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;

public class Player : MonoBehaviour, IDyinable
{
    public static Player instance = null; 

    private PlayerDialogue dialogue;
    public PlayerDialogue Dialogue => dialogue;
    private PlayerInteract interact;
    public PlayerInteract Interact => interact;
    private PlayerInventory inventory;
    public PlayerInventory Inventory => inventory;

    public bool HandsFull => inventory.HandsFull;
    public Item pickedItem => inventory.Item;
    public bool onDialogue => dialogue.OnDialogue;
    private bool isDead = false;
    [SerializeField] float DeathDelay = 10f;
    [SerializeField] private UnityEvent OnStartDeath;
    [SerializeField] private UnityEvent OnEndDeath;
    public TMP_Text deathText;

    private float deathTime = 0;
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
    [ProButton]
    public void Die()
    {
        OnStartDeath.Invoke();
        isDead = true;
        StartCoroutine(DeathCoroutine());
        deathTime = DeathDelay;
    }
    void Update()
    {
        if(isDead)
        {
            deathTime -= Time.deltaTime;
            deathText.text = Convert.ToInt32(deathTime).ToString();
        }
    }
    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(DeathDelay);
        OnEndDeath.Invoke();
        isDead = false;
    }
    public bool IsDead() => isDead;

}
