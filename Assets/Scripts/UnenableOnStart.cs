using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnenableOnStart : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false);
    }
}
