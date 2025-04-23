using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CamKeyValuePair
{
    public string key;
    public string value;
}


[CreateAssetMenu(fileName = "Data", menuName = "Data/ButtonsCamCouples", order = 1)]
public class CamDatas : ScriptableObject
{
    public List<CamKeyValuePair> J1BC = new List<CamKeyValuePair>();
    public List<CamKeyValuePair> J2BC = new List<CamKeyValuePair>();

    public List<Material> CamMaterialJ1 = new List<Material>();
    public List<Material> CamMaterialJ2 = new List<Material>();

    public Dictionary<string, string> GetJ1Dictionary()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        foreach (CamKeyValuePair pair in J1BC)
        {
            dict[pair.key] = pair.value;
        }
        return dict;
    }
 
    public Dictionary<string, string> GetJ2Dictionary()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        foreach (CamKeyValuePair pair in J2BC)
        {
            dict[pair.key] = pair.value;
        }
        return dict;
    }
}

