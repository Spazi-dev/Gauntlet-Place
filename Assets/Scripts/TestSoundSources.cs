using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSoundSources : MonoBehaviour
{
    AudioSource[] audioSources;
    
    void Awake()
    {
        audioSources = GetComponents<AudioSource>();
    }

    public void PlaySounds()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].Play();
        }
    }
}
