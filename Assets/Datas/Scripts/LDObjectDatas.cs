using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LDObject
{
    public string name;
    public string Tag;
}

[CreateAssetMenu(fileName = "Data", menuName = "Data/LDObjectDatas", order = 1)]
public class LDObjectDatas : ScriptableObject
{
    public List<LDObject> LDOBjectList = new List<LDObject>();
}
