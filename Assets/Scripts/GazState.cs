using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazState : MonoBehaviour
{
    public bool InGaz;

    private void Update()
    {
        if (InGaz)
        {
            Debug.Log("AHHHHHHHHHHHHH");
        }
    }
}
