using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace DialogueSystem{
public class DialogueLine : DialogueBaseClass
{
    private TMP_Text textHolder;
    [Header("Options")]
    //[SerializeField] private string input;
    [SerializeField] private Color textColor;
    [SerializeField] private TMP_FontAsset textFont;
    
    

    private void Awake()
    {
        textHolder = GetComponent<TMP_Text>();
    }
    public void WriteText(string text, AudioClip sound, float delay)
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine(WriteText(text, textHolder, textColor, textFont, delay, sound));
    }
}
}