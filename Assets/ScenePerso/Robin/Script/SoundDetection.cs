using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDetection : MonoBehaviour
{
    GameObject playerRef;
    public PlayerSound playerSound;
    public EnemyPathway enemyPathway;


    public void Update()
    {
        if (playerRef == null)
        {
            playerRef = GameObject.FindGameObjectWithTag("Player");
        }
        float distanceToTarget = Vector3.Distance(transform.position, playerSound.walkSound.transform.position);

        if(distanceToTarget < playerSound.walkSound.maxDistance && playerSound.walkSound.isPlaying)
        {
            Debug.Log("Entendu, j'arriiiiiiiive, hehehehe");
            enemyPathway.enemy.destination = playerRef.transform.position;
        }
    }
}
