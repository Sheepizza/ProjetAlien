using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiegeticSoundManager : MonoBehaviour
{
    [SerializeField]
    DiegeticsSoundsDatas diegeticsSoundsDatas;

    public Dictionary<string, (AudioClip audioClip, float range)> diegeticsSounds = new Dictionary<string, (AudioClip audioClip, float range)>();

    private static DiegeticSoundManager instance = null;
    public static DiegeticSoundManager Instance => instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

        GenerateDictionary();
    }

    void GenerateDictionary()
    {

        foreach (var item in diegeticsSoundsDatas.AudiosStruct)
        {
            diegeticsSounds.Add(item.Key, (item.AudioClip, item.Range));
        }
    }
}
