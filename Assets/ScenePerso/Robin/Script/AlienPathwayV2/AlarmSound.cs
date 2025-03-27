using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmSound : MonoBehaviour
{
    public AudioSource alarm;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            alarm.Play();
        }
    }
}
