using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDetection : MonoBehaviour
{
    GameObject playerRef;
    public PlayerSound playerSound;
    AudioSource[] audioSources;
    public AIManager manager;
    public AudioDatas AudioDatas;

    public void Update()
    {
        if (playerRef == null)
        {
            playerRef = GameObject.FindGameObjectWithTag("Player");
        }

        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].clip = AudioDatas.audioParams[i].Clip;
            audioSources[i].maxDistance = AudioDatas.audioParams[i].MaxRange;
            audioSources[i].volume = AudioDatas.audioParams[i].Volume;
                
                float distanceToTarget = Vector3.Distance(transform.position, audioSources[i].transform.position);
            if (distanceToTarget < audioSources[i].maxDistance)
            {
                manager.enemy.destination = audioSources[i].transform.position;
            }
        }
    }
}
