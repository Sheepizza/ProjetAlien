using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("CoroutineRunner");
                instance = obj.AddComponent<CoroutineRunner>();
                DontDestroyOnLoad(obj); // Assure que l'objet persiste entre les scènes
            }
            return instance;
        }
    }
}
