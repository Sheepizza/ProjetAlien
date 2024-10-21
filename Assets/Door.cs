using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    bool open = false;
    public void OpenDoor()
    {
        if (!open)
        {
            open = true;
            transform.Translate(Vector3.up * 4);
        }
        else
        {
            open = false;
            transform.Translate(Vector3.up * -4);
        }
    }
}
