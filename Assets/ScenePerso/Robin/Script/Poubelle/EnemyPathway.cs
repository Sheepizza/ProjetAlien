using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.AI.Navigation;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPathway : NetworkBehaviour
{
    public List<GameObject> pathways = new List<GameObject>();
    public NavMeshAgent enemy;
    int pathChosen;
    public bool pathwayOver;
    public int pathwayTiming;
    int pathwayCountdown;
    Vector3 roomPosition;
    GameObject actualRoom;


    // Start is called before the first frame update
    public void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        pathwayOver = true;
        pathwayCountdown = pathwayTiming;
    }

    public void FindRoom()
    {
        pathwayOver = false;
        pathChosen = Random.Range(0, pathways.Count);
        enemy.destination = pathways[pathChosen].transform.position;
        roomPosition = enemy.destination;
        actualRoom = pathways[pathChosen];
        StartCoroutine(Pathway());    
    }
    IEnumerator Pathway()
    {
        while(pathwayCountdown > 0)
        {
            if (enemy.transform.position.z == roomPosition.z && enemy.transform.position.x == roomPosition.x)
            {
                StartCoroutine(PathwayTimer());
            }
            if (enemy.destination.z == enemy.transform.position.z && enemy.destination.x == enemy.transform.position.x)
            {
                enemy.destination = actualRoom.transform.GetChild(Random.Range(0,actualRoom.transform.childCount)).position;
            }
            yield return new WaitForSeconds(2);
        }
        if (pathwayCountdown <= 0)
        {
            pathwayOver = true;
            pathwayCountdown = pathwayTiming;
        }
    }
    IEnumerator PathwayTimer()
    {
        while (pathwayCountdown != 0)
        {
            pathwayCountdown--;
            yield return new WaitForSeconds(1);
        }
    }
}
