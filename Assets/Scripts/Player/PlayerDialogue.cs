using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDialogue : MonoBehaviour
{
    [SerializeField] private TMP_InputField InputField;
    [SerializeField] private UnityEvent OnStartDialogue;
    [SerializeField] private UnityEvent OnEndDialogue;
    [SerializeField] private DialogueLine dialogueLine;
    public bool OnDialogue => onDialogue;
    private bool onDialogue = false;
    private Npc npcInDialog;
    public void StartDialogue(NpcDescription npc)
    {
        onDialogue = true;
        npcInDialog = npc.gameObject.GetComponent<Npc>();
        npcInDialog.StartDialogue();
        InputField.onEndEdit.AddListener(npc.EnterAsk);
        OnStartDialogue.Invoke();
    }
    
    public void EndDialogue()
    {
        npcInDialog.EndDialogue();
        OnEndDialogue.Invoke();
        InputField.onEndEdit.RemoveAllListeners();
        npcInDialog = null;
        onDialogue = false;
    }
    public void WriteText(string text, AudioClip sound, float delay)
    {
        dialogueLine.WriteText(text,sound,delay);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && OnDialogue)
        {
            EndDialogue();
        }
    }
}
