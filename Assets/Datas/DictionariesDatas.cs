using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyValuePair
{
    public string key;
    public string value;
}

[CreateAssetMenu(fileName = "Data", menuName = "Data/DictionariesDatas", order = 1)]
public class DictionariesDatas : ScriptableObject
{
    public List<KeyValuePair> J1BD = new List<KeyValuePair>();
    public List<KeyValuePair> J2BD = new List<KeyValuePair>();

    public Dictionary<string, string> GetJ1Dictionary()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        foreach (KeyValuePair pair in J1BD)
        {
            dict[pair.key] = pair.value;
        }
        return dict;
    }

    public Dictionary<string, string> GetJ2Dictionary()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        foreach (KeyValuePair pair in J2BD)
        {
            dict[pair.key] = pair.value;
        }
        return dict;
    }
}
