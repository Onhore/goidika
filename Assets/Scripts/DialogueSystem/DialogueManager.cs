using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public int delay = 10;

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
        if (initiator.OnDialogue || target.OnDialogue)
            return;
        initiator.StartDialogue(target.gameObject);
        target.StartDialogue(initiator.gameObject);
        NpcDescription initiatorDescription = initiator.GetComponent<NpcDescription>();
        NpcDescription targetDescription = target.GetComponent<NpcDescription>();
        string init = "[dialogue]: Вы начинаете диалог с" + target.name;
        string message = await initiatorDescription.Request(init);
        initiatorDescription.dialogue.ShowDialogueCloud(message);
        string response;
        Debug.Log(init);
        while (initiator.OnDialogue && target.OnDialogue)
        {
            response = await targetDescription.Request("[" + initiatorDescription.name + "]: " + message);
            Debug.Log("[" + initiatorDescription.name + "]: " + message);
            targetDescription.dialogue.ShowDialogueCloud(response);
            message = await initiatorDescription.Request("[" + targetDescription.name + "]: " + response);
            Debug.Log("[" + targetDescription.name + "]: " + response);
            initiatorDescription.dialogue.ShowDialogueCloud(message);

            // Добавляем задержку перед началом нового запроса
            await Task.Delay(1000*delay); // Задержка в 1 секунду (1000 миллисекунд)
        }
        Debug.Log("Close");
        initiator.EndDialogue();
        target.EndDialogue();
    }
}