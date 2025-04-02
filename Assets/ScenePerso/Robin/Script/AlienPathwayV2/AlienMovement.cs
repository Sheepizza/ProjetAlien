using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Mirror;
using Mono.CecilX;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AlienMovement : NetworkBehaviour
{
    private NavMeshAgent enemyNavMesh;
    private Animator animator;
    private Rigidbody _rb;

    void Start()
    {
        enemyNavMesh = GetComponent<NavMeshAgent>();
        FOV = GetComponent<FieldOfView>();
        animator = GetComponent<Animator> ();
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Debug.Log("le son est détecté ?" + soundDetected);

        if(_rb.velocity.magnitude > 0.1f)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }


        if(!soundDetected)
        {
            SoundDetection();
        }
        if(soundDetected)
        {
            SoundHunting();
        }
        if(playerRef == null)
        {
            playerRef = GameObject.Find("PlayerPrefab [connId=0]");
        }
        if(playerRef != null && !inPatrol && !hunting && !soundDetected)
        {
            StartCoroutine(FindRoom());
        }
        //Debug.Log(pathwayCountdown);
        
        if(FOV.canSeePlayer && !soundDetected)
        {
            if (stopHuntingCoroutine != null)
            {
                StopCoroutine(stopHuntingCoroutine);
            }
            StopCoroutine(StopingHunt());
            hunting = true;
            
            if(huntingCoroutine == null)
            {
                huntingCoroutine = StartCoroutine(Hunting());
            }
        }
        else
        {
            stopHuntingCoroutine = StartCoroutine(StopingHunt());
        }

        //==> sert pour le WaitUntil
    }

#region Patrouille
Coroutine pathwayCountdownCoroutine;
    private GameObject playerRef;
    public List<GameObject> rooms = new List<GameObject>();
    bool inPatrol = false;
    bool isArrived = false;
    int actualRoom;
    int pathwayCountdown = 20;
    public int pathwayTiming;
    IEnumerator FindRoom()
    {
        pathwayCountdownCoroutine = null;
        inPatrol = true;
        int pathChosen = Random.Range(0, rooms.Count);
        actualRoom = pathChosen;
        enemyNavMesh.destination = rooms[actualRoom].transform.position;
        

        while(inPatrol)
        {
            float distanceToDestination = Vector3.Distance(transform.position, enemyNavMesh.destination);

            if (distanceToDestination < 0.5f)  // Tolérance de 0.5 unités
            {
                if(pathwayCountdownCoroutine == null)
                {
                    pathwayCountdownCoroutine = StartCoroutine(PathwayCountdown());
                }
                isArrived = true;
            }
            else
            {
                isArrived = false;
            }

            if (pathwayCountdown <= 0)
            {
                pathwayCountdown = pathwayTiming;
                inPatrol = false;
            }

            if (isArrived)
            {
                enemyNavMesh.destination = rooms[actualRoom].transform.GetChild(Random.Range(0, rooms[actualRoom].transform.childCount)).position;
                isArrived = false;
            }

            yield return null;
        }
    }

    IEnumerator PathwayCountdown()
    {
        //Debug.Log("Debut Patrouille ma gueule");
        while (pathwayCountdown > 0)
        {
            pathwayCountdown--;
            yield return new WaitForSeconds(1);
        }
    }
    #endregion

#region HuntState
FieldOfView FOV;
Coroutine huntingCoroutine;
Coroutine stopHuntingCoroutine;
bool hunting = false;
public int escapeTiming;
IEnumerator Hunting()
{
    inPatrol = false;
    StopCoroutine(FindRoom());
    while(hunting)
    {
    enemyNavMesh.destination = playerRef.transform.position;
    yield return null;
    }
}

IEnumerator StopingHunt()
{
    if(!FOV.canSeePlayer)
    {
        yield return new WaitForSeconds(escapeTiming);
        hunting = false;
        huntingCoroutine = null;
    }

}
#endregion

#region SoundDetection
bool soundDetected = false;
public LayerMask soundSourceLayer;
public LayerMask soundSourceBreakable;
private Vector3 target;
void SoundDetection()
{
    Collider[] soundSources = Physics.OverlapSphere(transform.position, 50f, soundSourceLayer);
    Transform mostIntenseSource = null;
    float highestIntensity = 0f;

    foreach(Collider source in soundSources)
    {
        Sound sound = source.GetComponent<Sound>();
        Debug.Log("Le son est joué ?" + sound.audioSource.isPlaying);

        if(sound != null)
        {
            float soundRange = sound.GetCurrentSoundRange();
            float distance = Vector3.Distance(transform.position, source.transform.position);

            if(soundRange > 0 && distance > 0 &&  distance <= soundRange)
            {
                
                float intensity = soundRange / distance;
                if (intensity > highestIntensity)
                {
                    highestIntensity = intensity;
                    mostIntenseSource = source.transform;
                }
            }
        }
    }
    if (mostIntenseSource != null)
    {   
        enemyNavMesh.destination = mostIntenseSource.position;
        target = mostIntenseSource.position;
        soundDetected = true;
    }
}
void SoundHunting()
{
    StopCoroutine(FindRoom());
    inPatrol = false;
    float distanceToSource = Vector3.Distance(transform.position, target);
    //Debug.Log(distanceToSource);

    if(distanceToSource < 2f)
    {
        soundDetected = false;
        Collider[] soundSources = Physics.OverlapSphere(transform.position, distanceToSource + 1, soundSourceBreakable);
        foreach (Collider source in soundSources)
        {
            source.gameObject.SetActive(false);
        }
    }
}
#endregion
}
