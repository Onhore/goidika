using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void StartDialogue(Npc initiator, Npc target)
    {
        initiator.StartDialogue(target.gameObject);
        target.StartDialogue(initiator.gameObject);
        NpcDescription initiatorDescription = initiator.GetComponent<NpcDescription>();
        NpcDescription targetDescription = target.GetComponent<NpcDescription>();
        string init = "[dialogue]: Вы начинаете диалог с" + target.name;
        string message = await initiatorDescription.Request(init);
        string response;
        Debug.Log(init);
        while (initiator.OnDialogue && target.OnDialogue)
        {
            
            response = await targetDescription.Request("[" + initiatorDescription.name + "]: " + message);
            Debug.Log("[" + initiatorDescription.name + "]: " + message);
            message = await initiatorDescription.Request("[" + targetDescription.name + "]: " + response);
            Debug.Log("[" + targetDescription.name + "]: " + response);

            //initiatorDescription.CheckCommand(response);

            
        }
        Debug.Log("Close");
        initiator.EndDialogue();
        target.EndDialogue();
    }
}