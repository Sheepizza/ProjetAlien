using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LDTarget
{
    public string name;
    public string Tag;

    public string ResultName;
}


[CreateAssetMenu(fileName = "Data", menuName = "Data/LDTargetDatas", order = 1)]
public class LDTargetDatas : ScriptableObject
{
    public List<LDTarget> LDTargetList = new List<LDTarget>();
}
