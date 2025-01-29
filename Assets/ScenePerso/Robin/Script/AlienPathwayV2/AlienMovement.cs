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
    public int actualRoom;
    int pathwayCountdown = 10;

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
            FindRoom();
        }

        // if(transform.position.x == enemyNavMesh.destination.x && transform.position.z == enemyNavMesh.destination.z)
        // {
        //     isArrived = true;
        // }

        // if(transform.position.x != enemyNavMesh.destination.x || transform.position.z != enemyNavMesh.destination.z)
        // {
        //     isArrived = false;
        // }

        //Debug.Log(isArrived);

        // ==> sert pour le WaitUntil
    }


    void FindRoom()
    {
        int pathChosen = Random.Range(0, rooms.Count);
        actualRoom = pathChosen;
        enemyNavMesh.destination = rooms[pathChosen].transform.position;
        inPatrol = true;
        StartCoroutine(RoomPathway());
    }

    IEnumerator RoomPathway()
    {
        yield return new WaitUntil(() => isArrived);
        //Peut-être un truc à jouer avec ça, intéressant mais ptit bug sur le booléen.
        Debug.Log("Arrived");
        enemyNavMesh.destination = rooms[actualRoom].transform.GetChild(Random.Range(0, rooms[actualRoom].transform.childCount)).position;
    }
}
