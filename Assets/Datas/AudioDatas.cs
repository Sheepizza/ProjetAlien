using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/AudioDatas", order = 1)]
public class AudioDatas : ScriptableObject
{
    public AudioParam[] audioParams;
}

[System.Serializable]
public class AudioParam
{
    public string Name;
    public AudioClip Clip;

    [Range(0, 1)]
    public float Volume;

    [Range(1, 50)]
    public float MaxRange;
}
