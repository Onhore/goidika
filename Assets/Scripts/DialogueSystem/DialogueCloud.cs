using System.Collections;
using System.Collections.Generic;
using com.cyborgAssets.inspectorButtonPro;
using TMPro;
using UnityEngine;

public class DialogueCloud : MonoBehaviour
{
    public GameObject panel;
    public float showTime = 3.0f;
    public TMP_Text dialogueText; 

    private Coroutine showCoroutine;
    [ProButton]
    public void ShowDialogueCloud(string text)
    {
        if (showCoroutine != null)
        {
            StopCoroutine(showCoroutine);
        }
        panel.SetActive(true);
        dialogueText.text = text;
        
        showCoroutine = StartCoroutine(HideDialogueCloudAfterDelay());
    }

    private IEnumerator HideDialogueCloudAfterDelay()
    {
        yield return new WaitForSeconds(showTime);
        HideDialogueCloud();
    }

    private void HideDialogueCloud()
    {
        panel.SetActive(false);
    }
    private void Update()
    {

        Vector3 direction = Camera.main.transform.position - transform.position;
        direction.y = 0; 
        transform.rotation = Quaternion.LookRotation(direction);
        
    }
}
