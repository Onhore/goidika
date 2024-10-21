using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;

public class NpcDescription : MonoBehaviour
{
    [TextArea(15,20)] public string Description = "";
    private List<ChatMessage> messages = new List<ChatMessage>();
    private string LastMessage = "";

    void Start()
    {
        SetContext(Description);
    }
    public void SetContext(string text)
    {
        ChatMessage newMessage = new ChatMessage();
        newMessage.Content = text;
        newMessage.Role = "system";

        messages.Add(newMessage);
    }

    public void Ask()
    {
        
    }
}
