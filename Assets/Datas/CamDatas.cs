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
    public List<KeyValuePair> J1BC = new List<KeyValuePair>();
    public List<KeyValuePair> J2BC = new List<KeyValuePair>();

    public Dictionary<string, string> GetJ1Dictionary()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        foreach (KeyValuePair pair in J1BC)
        {
            dict[pair.key] = pair.value;
        }
        return dict;
    }

    public Dictionary<string, string> GetJ2Dictionary()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        foreach (KeyValuePair pair in J2BC)
        {
            dict[pair.key] = pair.value;
        }
        return dict;
    }
}

