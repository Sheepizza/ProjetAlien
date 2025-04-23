using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/ButtonsCamCouples", order = 1)]
public class AmbiantSoundsDatas : ScriptableObject
{
    public List<AudioSource> audioSources;
}
