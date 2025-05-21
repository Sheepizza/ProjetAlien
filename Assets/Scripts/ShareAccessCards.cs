using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareAccessCards : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponentInChildren<PickUpManager>().ShareKeys();
        }
    }
}
