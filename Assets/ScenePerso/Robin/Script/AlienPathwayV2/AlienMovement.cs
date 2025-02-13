using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AlienMovement : NetworkBehaviour
{
    private NavMeshAgent enemyNavMesh;
    void Start()
    {
        enemyNavMesh = GetComponent<NavMeshAgent>();
        FOV = GetComponent<FieldOfView>();
    }

    void Update()
    {
        if(playerRef == null)
        {
            playerRef = GameObject.Find("Player1");
        }
        if(playerRef != null && !inPatrol && !hunting)
        {
            StartCoroutine(FindRoom());
        }
        //Debug.Log(pathwayCountdown);
        
        if(FOV.canSeePlayer)
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
        Debug.Log("Debut Patrouille ma gueule");
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

}
