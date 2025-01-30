using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AlienMovement : NetworkBehaviour
{
    private GameObject playerRef;
    private NavMeshAgent enemyNavMesh;
    public List<GameObject> rooms = new List<GameObject>();

    bool inPatrol = false;
    bool isArrived = false;
    int actualRoom;
    int pathwayCountdown = 20;
    public int pathwayTiming;

    void Start()
    {
        enemyNavMesh = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(playerRef == null)
        {
            playerRef = GameObject.Find("Player1");
        }
        if(playerRef != null && !inPatrol)
        {
            StartCoroutine(FindRoom());
        }
     
        Debug.Log(inPatrol);
        Debug.Log(pathwayCountdown);

        //==> sert pour le WaitUntil
    }


    IEnumerator FindRoom()
    {
        inPatrol = true;
        int pathChosen = Random.Range(0, rooms.Count);
        actualRoom = pathChosen;
        enemyNavMesh.destination = rooms[actualRoom].transform.position;


        while(inPatrol)
        {
            float distanceToDestination = Vector3.Distance(transform.position, enemyNavMesh.destination);
            float distanceToActualRoom = Vector3.Distance(transform.position, rooms[actualRoom].transform.position);


            if (distanceToDestination < 0.5f)  // Tolérance de 0.5 unités
            {
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
                Debug.Log("Arrived");
                enemyNavMesh.destination = rooms[actualRoom].transform.GetChild(Random.Range(0, rooms[actualRoom].transform.childCount)).position;
                isArrived = false;
            }


            yield return null;
        }
    }

    IEnumerator PathwayCountdown()
    {
        while (pathwayCountdown > 0)
        {
            pathwayCountdown--;
            yield return new WaitForSeconds(1);
        }
    }
}
