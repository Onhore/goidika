using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAI;
using UnityEngine;

public class ChatGPTManager : MonoBehaviour
{
    public static ChatGPTManager instance = null; 
    [SerializeField] private List<GameObject> npcs; 
    private OpenAIApi openAI = new OpenAIApi();
    //private List<ChatMessage> messages = new List<ChatMessage>();
    [SerializeField] [TextArea(15, 20)] private string generalDescription;
    public string GeneralDescription => generalDescription;
    private void Awake()
    {
        if (instance == null) 
            instance = this; 
        else if(instance == this)
            Destroy(gameObject); 
    }

    public async Task<string> AskChatGPT(string newText, List<ChatMessage>messages)
    {
        ChatMessage newMessage = new ChatMessage();
        newMessage.Content = newText;
        newMessage.Role = "user";

        messages.Add(newMessage);

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = messages;
        request.Model = "gpt-4-turbo";
        request.MaxTokens = 1000;
//"gpt-3.5-turbo"
//"gpt-4-turbo"
        var response = await openAI.CreateChatCompletion(request);

        if (response.Choices != null && response.Choices.Count > 0)
        {
            var chatResponse = response.Choices[0].Message;
            messages.Add(chatResponse);

            return chatResponse.Content;
        }
        return "Ничего";
    }
    public void BroadcastMessageWithReaction(string text, GameObject[] _npcs)
{
    foreach (GameObject npc in npcs)
    {
        if (Array.Exists(_npcs, element => element == npc))
            continue;
        npc.GetComponent<NpcDescription>()?.ReactSystemMessage(text);
        Debug.Log(npc.name + text);
    }
}

public void BroadcastMessage(string text, GameObject[] _npcs)
{
    foreach (GameObject npc in npcs)
    {
        if (Array.Exists(_npcs, element => element == npc))
            continue;
        npc.GetComponent<NpcDescription>()?.AddSystemMessage(text);
    }
}

}
