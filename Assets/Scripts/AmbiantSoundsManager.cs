using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbiantSoundsManager : MonoBehaviour
{
    public AmbiantSoundsDatas ambiantSoundsDatas;
    public AudioSource currentAudioSource;

    bool goPlaySound = false;

    public IEnumerator SoundCoolDown()
    {
        float currentTime = 0;
        float Timelimit = 30;
        float TimeRandomCheck = 10;
        

        if(goPlaySound)
        {
            PlayRandomSound();
        }


        if(!goPlaySound)
        {
            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
            if(currentTime >= Timelimit && currentTime%10 == TimeRandomCheck%10)
            {
                int randomNumber = Random.Range(0, 10);
                if(randomNumber == 1)
                {
                    goPlaySound = true;
                }
            }
        }
    }

    void PlayRandomSound()
    {
        int RandomSoundNumber = Random.Range(0, ambiantSoundsDatas.audioSources.Count);
        for(int i = 0; i < ambiantSoundsDatas.audioSources.Count; i++)
        {
            if(RandomSoundNumber == i)
            {
                currentAudioSource = ambiantSoundsDatas.audioSources[i];
                currentAudioSource.Play();
            }
        }
        //Quand le son est fini de jouer
        goPlaySound = false;
    }
}
