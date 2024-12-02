using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDetection : MonoBehaviour
{
    AudioSource walkSound;
    EnemyPathway enemyPathway;

    void Start()
    {
        enemyPathway = GetComponent<EnemyPathway>();
    }

    void Update()
    {
        if(walkSound == null)
        {
            walkSound = GameObject.Find("WalkSound").GetComponent<AudioSource>();
        }

        float distanceToTarget = Vector3.Distance(transform.position, walkSound.transform.position);

        if(distanceToTarget < walkSound.maxDistance && walkSound.isPlaying)
        {
            Debug.Log("Entendu !");
            enemyPathway.enemy.destination = walkSound.transform.position;
        }
    }
}
