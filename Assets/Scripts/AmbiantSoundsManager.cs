using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbiantSoundsManager : MonoBehaviour
{
    public AmbiantSoundsDatas ambiantSoundsDatas;
    public AudioSource currentClip;

    public List<AudioSource> audioSources = new List<AudioSource>();
    public Coroutine coroutine;

    public bool goPlaySound = false;
    [SerializeField]
    float currentTime = 0;

    void Awake()
    {
        AssignSound();
    }

    public void AssignSound()
    {        
        for(int i = 0; i < ambiantSoundsDatas.ambiantSound.Count; i++)
        {
            AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
            audioSources.Add(new AudioSource());
            audioSources[i] = newAudioSource;
            audioSources[i].clip = ambiantSoundsDatas.ambiantSound[i].Clip;
            //Debug.Log("Assigning Sounds " + i);
        }
    }

    void Update()
    {
        if(coroutine == null && goPlaySound == true)
        {
            Debug.Log("Try Start Coroutine");
            coroutine = StartCoroutine(SoundCoolDown());
        }
    }

    public void StartAmbiantSound()
    {
        goPlaySound = true;
        Debug.Log(goPlaySound);
    }

    public IEnumerator SoundCoolDown()
    {

        float Timelimit = 2;
        Debug.Log("Coroutine Start");
        if(!goPlaySound)
        {
            while(currentTime < Timelimit)
            {
                currentTime += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            Debug.Log("IcanPLay");
            int randomNumber = Random.Range(0, 5);
            Debug.Log(randomNumber );
            if(randomNumber == 1)
            {
                PlayRandomSound();
            }
            currentTime = 0;
        }
        coroutine = null;
    }

    void PlayRandomSound()
    {
        Debug.Log("TryToPlay");
        int RandomSoundNumber = Random.Range(0, ambiantSoundsDatas.ambiantSound.Count);
        Debug.Log(RandomSoundNumber);
        for(int i = 0; i < ambiantSoundsDatas.ambiantSound.Count; i++)
        {
            if(RandomSoundNumber == i)
            {
                currentClip = audioSources[i];
                Debug.Log(currentClip.clip.name);
                currentClip.Play();
                Debug.Log("SoundIsPlayed");
            }
        }
    }
}
