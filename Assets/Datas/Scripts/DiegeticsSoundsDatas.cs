using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/DiegeticsSounds", order = 1),
    System.Serializable]
public class DiegeticsSoundsDatas : ScriptableObject
{
    public AudioStruct[] AudiosStruct;
}

[System.Serializable]
public struct AudioStruct
{
    public string Key;
    public AudioClip AudioClip;
    public float Range;
}
