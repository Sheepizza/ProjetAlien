using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundTrigger : MonoBehaviour
{
    [Header("Choix du son à jouer")]
    public AudioSource sonAJouer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (sonAJouer != null)
            {
                sonAJouer.Play();
                Destroy(other.gameObject);
            }

        }
    }
}
