using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Data", menuName = "Data/AmbiantSoundDatas", order = 1)]
public class AmbiantSoundsDatas : ScriptableObject
{
    public List<AmbiantSound> ambiantSound;
}

[System.Serializable]
public class AmbiantSound
{
    public string name;
    public AudioSource audioSource;
    public AudioClip Clip;
    [Range(0,1)]
    public float volume;
}