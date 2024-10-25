using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDialogue : MonoBehaviour
{
    [SerializeField] private TMP_InputField InputField;
    [SerializeField] private UnityEvent OnStartDialogue;
    [SerializeField] private UnityEvent OnEndDialogue;
    public bool OnDialogue => onDialogue;
    private bool onDialogue = false;
    public void StartDialogue(NpcDescription npc)
    {
        onDialogue = true;
        InputField.onEndEdit.AddListener(npc.Ask);
        OnStartDialogue.Invoke();
    }
    public void EndDialogue()
    {
        OnEndDialogue.Invoke();
        InputField.onEndEdit = null;
        onDialogue = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && OnDialogue)
        {
            EndDialogue();
        }
    }
}
