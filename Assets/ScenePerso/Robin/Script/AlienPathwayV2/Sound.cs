using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public float range;

    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public float GetCurrentSoundRange()
    {
        return audioSource != null && audioSource.isPlaying ? range : 0f;
    }
}
