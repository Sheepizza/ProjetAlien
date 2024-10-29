using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPos : MonoBehaviour
{
    public Transform CamHolder;

    // Update is called once per frame
    void Update()
    {
        transform.position = CamHolder.position;
    }
}
