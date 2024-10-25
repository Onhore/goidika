using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using TMPro;

public class NpcDescription : MonoBehaviour
{
    [TextArea(15,20)] public string Description = "";
    private List<ChatMessage> messages = new List<ChatMessage>();
    public ChatGPTManager ChatGPT;
    private string LastMessage = "";

    public TMP_Text Diag;

    void Start()
    {
        SetContext(Description);
        Debug.Log(this + " " + Description);
    }
    private void SetContext(string text)
    {
        ChatMessage newMessage = new ChatMessage();
        newMessage.Content = text;
        newMessage.Role = "system";

        messages.Add(newMessage);
    }

    public async void Ask(string text)
    {
        string answ = await ChatGPT.AskChatGPT(text, messages);
        Diag.text = answ;
        Debug.Log(answ);
    }
}
