using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using TMPro;
using System.Text.RegularExpressions;
using com.cyborgAssets.inspectorButtonPro;
using System.Threading.Tasks;

[RequireComponent(typeof(Npc))]
public class NpcDescription : MonoBehaviour
{
    [TextArea(15, 20)] public string Description = "";
    private List<ChatMessage> messages = new List<ChatMessage>();
    public ChatGPTManager ChatGPT;
    private string LastMessage = "";
    [SerializeField] private AudioClip sound;
    [SerializeField] private float delay;

    public DialogueCloud dialogue;
    //public TMP_Text Diag;
    private Npc npc;

    void Awake()
    {
        npc = GetComponent<Npc>();
    }

    void Start()
    {
        SetContext(Description + ChatGPT.GeneralDescription);
        //Debug.Log(this + " " + Description);
    }

    private void SetContext(string text)
    {
        ChatMessage newMessage = new ChatMessage();
        newMessage.Content = text;
        newMessage.Role = "system";

        messages.Add(newMessage);
        Debug.Log(this + text);
    }
    public void AddSystemMessage(string text) => SetContext(text);
    public async void ReactSystemMessage(string text)
    {
        //Debug.Log(text);
        string answ = await ChatGPT.AskChatGPT("[system]: " + text, messages);
        Debug.Log(this.gameObject +" "+ answ);
        CheckCommand(answ);
    }
    public async void Ask(string text)
    {
        string answ = await ChatGPT.AskChatGPT(text, messages);
        Debug.Log(this.gameObject +" "+ answ);
        answ = CheckCommand(answ);
        Player.instance.GetComponent<PlayerDialogue>().WriteText(answ, sound, delay);
        //Diag.text = answ;
        //Debug.Log(answ);
    }
    public async Task<string> Request(string request)
    {
        string response = await ChatGPT.AskChatGPT(request, messages);
        //Debug.Log(this.gameObject + " " + response);
        CheckCommand(response);
        return response;
    }
    public void EnterAsk(string text)
    {
        Ask("[Player]: " + text);
    }
    [ProButton]
    public string CheckCommand(string text)
    {
        // Используем выражение для поиска команды в круглых скобках
        Match match = Regex.Match(text, @"\((.*?)\)");
        if (match.Success)
        {
            string commandText = match.Groups[1].Value;

            // Разделяем текст на команду и атрибут
            string[] parts = commandText.Split(' ');
            if (parts.Length > 2)
            {
                Debug.LogError("Неверный формат команды: " + commandText);
                return text;
            }

            string command = parts[0];
            //string attribute = parts[1];

            switch (command)
            {
                case "Идти":
                    if (parts.Length < 2)
                       { npc.Go();
                        break;}
                    GlobalLists.Place place = GlobalLists.PlaceList.instance.FindPlace(parts[1]);
                    if (place != null)
                    {
                        if (npc.OnDialogue)
                            npc.EndDialogueEvent += () => npc.Go(place);
                        else
                            npc.Go(place);
                    }
                    break;
                case "Следовать":
                    GameObject ftarget = GlobalLists.MobList.instance.FindMob(parts[1]).gameObject;
                    //Debug.Log(parts[1]);
                    if (ftarget != null)
                    {
                        if (npc.OnDialogue)
                            npc.EndDialogueEvent += () => npc.Follow(ftarget);
                        else
                            npc.Follow(ftarget);
                    }
                    break;
                case "Подойти":
                    GameObject target = GlobalLists.MobList.instance.FindMob(parts[1]).gameObject;
                    //Debug.Log(parts[1]);
                    if (target != null)
                    {
                        if (npc.OnDialogue)
                            npc.EndDialogueEvent += () => npc.ApproachTo(target);
                        else
                            npc.ApproachTo(target);
                    }
                    break;
                case "ПрекратитьСледование":
                    npc.StopFollow();
                    break;
                case "ПрекратитьИдти":
                    npc.StopGo();
                    break;
                case "Атаковать":
                    target = GlobalLists.MobList.instance.FindMob(parts[1]).gameObject;
                    //Debug.Log(target);
                    if (target != null)
                    {
                        if (npc.OnDialogue)
                        {
                            npc.EndDialogueEvent+= () => npc.Attack(target);
                            if (target == Player.instance.gameObject && Player.instance.onDialogue)
                                Player.instance.GetComponent<PlayerDialogue>().EndDialogue();
                        }
                        else
                            npc.Attack(target);
                    }
                    break;
                case "СлучайнаяХодьба":
                    npc.RandomWalk();
                    break;
                case "НачатьДиалог":
                    if (parts.Length < 2)
                            break;
                    target = GlobalLists.MobList.instance.FindMob(parts[1]).gameObject;
                    //Debug.Log(parts[1]);
                    if (target != null)
                    {
                        if (!npc.OnDialogue && target != Player.instance.gameObject && npc.gameObject != target && !target.GetComponent<Npc>().OnDialogue)
                            DialogueManager.instance.StartDialogue(npc, target.GetComponent<Npc>());
                    }
                    break;
                case "ЗакончитьДиалог":
                    if (npc.OnDialogue) 
                        npc.EndDialogue();
                    break;
                case "Ничего":
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
            return text;
        }
    }

    
}