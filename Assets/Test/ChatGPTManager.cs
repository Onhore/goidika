using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAI;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine;

public class ChatGPTManager : MonoBehaviour
{
    private OpenAIApi openAI = new OpenAIApi();
    //private List<ChatMessage> messages = new List<ChatMessage>();

    public async Task<string> AskChatGPT(string newText, List<ChatMessage>messages)
    {
        ChatMessage newMessage = new ChatMessage();
        newMessage.Content = newText;
        newMessage.Role = "user";

        messages.Add(newMessage);

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = messages;
        request.Model = "gpt-3.5-turbo";

        var response = await openAI.CreateChatCompletion(request);

        if (response.Choices != null && response.Choices.Count > 0)
        {
            var chatResponse = response.Choices[0].Message;
            messages.Add(chatResponse);

            return chatResponse.Content;
        }
        return null;
    }


}
