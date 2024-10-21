using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerDialogue : MonoBehaviour
{
    public GameObject DialoguePanel;
    public TMP_InputField InputF;

    void Start()
    {
        DialoguePanel.SetActive(false);
        InputF.gameObject.SetActive(false);
    }
    public void StartDialogue()
    {
        DialoguePanel.SetActive(true);
        InputF.gameObject.SetActive(true);
        InputF.Select();
    }
}
