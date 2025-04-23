using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class Alien : MonoBehaviour
{
    #region State Machine Variables
    public AlienStateMachine StateMachine { get; set; }
    public PatrolState patrolState { get; set; }
    public HuntState huntState { get; set; }

    #endregion

    #region AnimationTriggerEvents
    #endregion

    #region components
    public NavMeshAgent enemyNavMesh;
    public FieldOfView FOV;
    Animator animator;
    Rigidbody _rb;
    public List<GameObject> rooms = new List<GameObject>();
    public List<GameObject> roomsAroundPlayer = new List<GameObject>();
    public GameObject losingCanva;
    public GameObject playerRef;
    #endregion

    #region other variables
    Coroutine pathwayCountdownCoroutine;

    bool inPatrol = false;
    bool isArrived = false;
    int actualRoom;
    public int pathwayCountdown = 20;
    public int pathwayTiming;
    #endregion

    private void Start()
    {
        StateMachine.Initialize(patrolState);


        enemyNavMesh = GetComponent<NavMeshAgent>();
        FOV = GetComponent<FieldOfView>();
        animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        rooms.AddRange(GameObject.FindGameObjectsWithTag("Room"));
        losingCanva.SetActive(false);
    }
    private void Awake()
    {
        StateMachine = new AlienStateMachine();
        patrolState = new PatrolState(this, StateMachine);
        huntState = new HuntState(this, StateMachine);
    }

    private void Update()
    {
        StateMachine._CurrentState.FrameUpdate();
        if (!soundDetected)
        {
            SoundDetection();
        }

        if (playerRef == null)
        {
            if (name == "MonsterCancerServer")
                playerRef = GameObject.Find("Player1");
            else
                playerRef = GameObject.Find("Player2");
        }
    }

    private void FixedUpdate()
    {
        StateMachine._CurrentState.PhysicsUpdate();
    }

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

        foreach (Collider source in soundSources)
        {
            Debug.Log(source);
            Sound sound = source.GetComponent<Sound>();

            Debug.Log("Le son est joué ?" + sound.audioSource.isPlaying);

            if (sound != null)
            {
                float soundRange = sound.GetCurrentSoundRange();
                float distance = Vector3.Distance(transform.position, source.transform.position);

                if (soundRange > 0 && distance > 0 && distance <= soundRange)
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

    #endregion
    #region Patrouille
    public IEnumerator FindRoom()
    {
        pathwayCountdownCoroutine = null;
        inPatrol = true;
        if (roomsAroundPlayer.Count != 0)
        {
            Debug.Log("Le monstre patrouille autour du joueur");
            int pathChosen = Random.Range(0, roomsAroundPlayer.Count);
            actualRoom = pathChosen;
            enemyNavMesh.destination = roomsAroundPlayer[actualRoom].transform.position;
        }
        else
        {
            Debug.Log("Le monstre patrouille aléatoirement");
            int rdmRoom = Random.Range(0, rooms.Count);
            actualRoom = rdmRoom;
            enemyNavMesh.destination = rooms[actualRoom].transform.position;
        }
        while (inPatrol)
        {
            float distanceToDestination = Vector3.Distance(transform.position, enemyNavMesh.destination);

            if (distanceToDestination < 0.5f)  // Tolérance de 0.5 unités
            {
                if (pathwayCountdownCoroutine == null)
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
}
    

#endregion