using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePauser : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public void Pause()
    {
        if (!audioSource.mute)
            audioSource.mute = true;
        else
            audioSource.mute = false;
    }
}
