using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance = null;
    public static AudioManager Instance => instance;
    public AudioDatas AudioDatas;
    
    private void Awake()
    {
        if(instance != null && instance !=this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public AudioClip GetClip(string name)
    {
        foreach (var param in AudioDatas.audioParams)
        {
            if (param.Name == name)
            {
                return param.Clip;
            }
        }
        return null;
    }

    public float GetRange(string name)
    {
        foreach(var param in AudioDatas.audioParams)
        {
            if(param.Name == name)
            {
                return param.MaxRange;
            }
        }
        return 0;
    }

    public float GetVolume(string name)
    {
        foreach (var param in AudioDatas.audioParams)
        {
            if(param.Name == name)
            {
                return param.Volume;
            }
        }
        return 0;
    }
}

