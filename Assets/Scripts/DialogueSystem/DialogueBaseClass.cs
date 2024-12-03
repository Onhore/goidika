using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using TMPro;

namespace DialogueSystem
{
public class DialogueBaseClass : MonoBehaviour
{
    protected IEnumerator WriteText(string input, TMP_Text textHolder, Color textColor, TMP_FontAsset textFont, float delay, AudioClip sound)
    {
        
        textHolder.color = textColor;
        textHolder.font = textFont;
        textHolder.text = "";

        for(int i =0; i<input.Length;i++)
        {
            textHolder.text += input[i];
            SoundManager.instance.PlaySound(sound);
            yield return new WaitForSeconds(delay);
        }
    }
}
}