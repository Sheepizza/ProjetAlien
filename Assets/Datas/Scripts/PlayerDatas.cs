using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/PlayerDatas", order = 1)]
public class PlayerDatas : ScriptableObject
{
    [Header("Speed")]
    public float Speed;
    public float InitRatio = 1;
    public float SprintRatio = 2f;
    public float CrouchRatio = 0.5f;
    public float LyingRatio = 0.25f;

    [Header("Environment")]
    public float GravityMultiplier = 10f;
}
