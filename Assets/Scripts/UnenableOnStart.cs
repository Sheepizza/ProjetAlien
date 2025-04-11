using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnenableOnStart : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }
}
