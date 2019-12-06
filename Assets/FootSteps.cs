using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip[] audioClips;

    public void Step()
    {
        audioSource.PlayOneShot(GetRandomClip());
    }

    public AudioClip GetRandomClip()
    {
        return audioClips[UnityEngine.Random.Range(0, audioClips.Length)];
    }
}
