using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using TMPro;
using System.Text.RegularExpressions;

public class NpcDescription : MonoBehaviour
{
    [TextArea(15, 20)] public string Description = "";
    private List<ChatMessage> messages = new List<ChatMessage>();
    public ChatGPTManager ChatGPT;
    private string LastMessage = "";
    [SerializeField] private AudioClip sound;
    [SerializeField] private float delay;
    //public TMP_Text Diag;
    private Npc npc;

    void Awake()
    {
        npc = GetComponent<Npc>();
    }

    void Start()
    {
        SetContext(Description);
        Debug.Log(this + " " + Description);
        //Debug.Log(CheckCommand("Привет путник! [Атаковать Player] dgd [Идти вперед] впыппыпыф фпфвпы ывп ып [Срать под себя]"));
    }

    private void SetContext(string text)
    {
        ChatMessage newMessage = new ChatMessage();
        newMessage.Content = text;
        newMessage.Role = "system";

        messages.Add(newMessage);
    }
    public void AddSystemMessage(string text) => SetContext(text);

    public async void Ask(string text)
    {
        string answ = await ChatGPT.AskChatGPT(text, messages);
        Debug.Log(answ);
        answ = CheckCommand(answ);
        Player.instance.GetComponent<PlayerDialogue>().WriteText(answ, sound, delay);
        //Diag.text = answ;
        Debug.Log(answ);
    }

    public string CheckCommand(string text)
    {
        // Используем регулярное выражение для поиска команды в квадратных скобках
        Match match = Regex.Match(text, @"\[(.*?)\]");
        if (match.Success)
        {
            string commandText = match.Groups[1].Value;

            // Разделяем текст на команду и атрибут
            string[] parts = commandText.Split(' ');
            if (parts.Length != 2)
            {
                Debug.LogError("Неверный формат команды: " + commandText);
                return text; // Возвращаем исходный текст, если формат команды неверен
            }

            string command = parts[0];
            string attribute = parts[1];

            // Проверяем команду и вызываем соответствующий метод
            switch (command)
            {
                case "Идти":
                    GlobalLists.Place place = GlobalLists.PlaceList.instance.FindPlace(attribute);
                    if (place != null)
                    {
                        npc.EndDialogueEvent += () => npc.Go(place);
                    }
                    break;
                case "Следовать":
                    GameObject target = GlobalLists.MobList.instance.FindMob(attribute);
                    if (target != null)
                    {
                        npc.EndDialogueEvent += () => npc.Follow(target);
                    }
                    break;
                case "ПрекратитьСледование":
                    npc.StopFollow();
                    break;
                case "ПрекратитьИдти":
                    npc.StopGo();
                    break;
                case "Атаковать":
                    //Attack(attribute);
                    break;
                case "ПроверитьИнвентарь":
                    //CheckInventory(attribute);
                    break;
                case "Закончить":
                    //Finish(attribute);
                    break;
                default:
                    //Debug.LogError("Неизвестная команда: " + command);
                    return text; // Возвращаем исходный текст, если команда неизвестна
            }

            // Возвращаем текст без команды
            return text.Replace(match.Value, "").Trim();
        }
        else
        {
            //Debug.LogError("Команда не найдена в тексте: " + text);
            return text; // Возвращаем исходный текст, если команда не найдена
        }
    }
}