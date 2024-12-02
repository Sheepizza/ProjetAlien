using System.Collections;
using System.Collections.Generic;
using Mirror.Examples.Billiards;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource walkSound;
    private Rigidbody rb;
    public void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void Update()
    {
        Debug.Log(walkSound.isPlaying);
        if(rb.velocity.magnitude > 0.1f)
            {
                walkSound.Play();
            }
        else
            {
                walkSound.Stop();
            }
    }
}
