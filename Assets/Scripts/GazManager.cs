using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponentInChildren<GazState>().InGaz = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponentInChildren<GazState>().InGaz = false;
        }
    }
}
