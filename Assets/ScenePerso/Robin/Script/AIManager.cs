using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{
    private Animator animator;

    [Header("Patrol Settings")]
    public List<GameObject> pathways = new List<GameObject>();
    public NavMeshAgent enemy;
    int pathChosen;
    public bool pathwayOver;
    public int pathwayTiming;
    int pathwayCountdown;
    Vector3 roomPosition;
    GameObject actualRoom;

    FieldOfView fov;

    public GameObject playerRef;
    bool isHunting;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<NavMeshAgent>();

        pathwayOver = true;
        pathwayCountdown = pathwayTiming;

        isHunting = false;
        fov = GetComponent<FieldOfView>();
    }
    #region State
    void Update()
    {
        if (playerRef == null)
        {
            playerRef = GameObject.FindGameObjectWithTag("Player");
        }

        if (fov.canSeePlayer)
        {
            isHunting = true;
            animator.SetTrigger("canChase");
            StopCoroutine(HuntStateTimer());
        }
        else if (!fov.canSeePlayer)
        {
            StartCoroutine(HuntStateTimer());
        }

        if (isHunting)
        {
            Hunt();
            animator.SetBool("isChasing", true);
        }
        else if (!isHunting)
        {
            if (pathwayOver && pathways.Count > 0)
            {
                FindRoom();
            }
        }

    }
    #endregion

    #region Pathway
    /// <summary>
    /// Assigne une salle à l'Alien où il doit aller
    /// </summary>
    public void FindRoom()
    {
        pathwayOver = false;
        pathChosen = Random.Range(0, pathways.Count);
        enemy.destination = pathways[pathChosen].transform.position;
        roomPosition = enemy.destination;
        actualRoom = pathways[pathChosen];
        StartCoroutine(Pathway());
    }
    
    
    /// <summary>
    /// Alien patrouille dans la room
    /// </summary>
    /// <returns></returns>
    IEnumerator Pathway()
    {
        while (pathwayCountdown > 0)
        {
            //L'Alien est arrivé dans la salle
            if (enemy.transform.position.z == roomPosition.z && enemy.transform.position.x == roomPosition.x)
            {
                StartCoroutine(PathwayTimer());
            }

            if (enemy.destination.z == enemy.transform.position.z && enemy.destination.x == enemy.transform.position.x)
            {
                enemy.destination = actualRoom.transform.GetChild(Random.Range(0, actualRoom.transform.childCount)).position;
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
    #endregion

    #region Hunt
    
    void Hunt()
    {
        pathwayOver = true;
        enemy.destination = playerRef.transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(Kill());
        }
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds(1);
        //playerRef.SetActive(false);
    }

    IEnumerator HuntStateTimer()
    {
        yield return new WaitForSeconds(3);
        isHunting = false;
        animator.SetTrigger("canChase");
    }
}

    #endregion

