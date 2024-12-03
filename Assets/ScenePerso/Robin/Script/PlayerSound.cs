using System.Collections;
using System.Collections.Generic;
using Mirror.Examples.Billiards;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource walkSound;
    private Rigidbody rb;
    public bool isWalking;
    public void Start()
    {
        walkSound.GetComponent<AudioSource>();
        rb = gameObject.GetComponent<Rigidbody>();

        walkSound.clip = AudioManager.Instance.GetClip("WalkSound");
        walkSound.maxDistance = AudioManager.Instance.GetRange("WalkSound");
        walkSound.volume = AudioManager.Instance.GetVolume("WalkSound");
    }

    public void Update()
    {
        if(rb.velocity.magnitude > 0.1f && !isWalking)
            {
                walkSound.Play();
                isWalking = true;
            }
        else if(rb.velocity.magnitude <= 0.1 && isWalking)
        {
            walkSound.Stop();
            isWalking = false;
        }
    }
}
