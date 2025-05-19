using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class Alien : NetworkBehaviour
{
    #region State Machine Variables
    public AlienStateMachine StateMachine { get; set; }
    public PatrolState patrolState { get; set; }
    public HuntState huntState { get; set; }
    public SearchingState searchingState { get; set; }
    public BaladeState baladeState { get; set; }

    #endregion

    #region AnimationTriggerEvents
    #endregion

    #region components

    AudioSource footstepAudio;
    public AudioClip[] audioClips;
    public AudioSource alienScream;
    public NavMeshAgent enemyNavMesh;
    public FieldOfView FOV;
    public Animator animator;
    Rigidbody _rb;
    public List<GameObject> rooms = new List<GameObject>();
    public List<GameObject> roomsAroundPlayer = new List<GameObject>();
    public GameObject losingCanva;
    public GameObject playerRef;
    #endregion

    #region other variables
    public Coroutine pathwayCountdownCoroutine;
    
    public bool inPatrol = false;
    public float enemyRange;
    public bool isArrived = false;
    public int actualRoom;
    public int pathwayCountdown = 20;
    public int pathwayTiming;
    public int escapeTiming;
    #endregion

    private void Start()
    {
        footstepAudio = GetComponent<AudioSource>();
        enemyNavMesh = GetComponent<NavMeshAgent>();
        FOV = GetComponent<FieldOfView>();
        animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        losingCanva.SetActive(false);

        StateMachine.Initialize(patrolState);

        
        //alienScream.clip = DiegeticSoundManager.Instance.diegeticsSounds["Alien_Scream"].audioClip;
    }
    private void Awake()
    {
        StateMachine = new AlienStateMachine();
        patrolState = new PatrolState(this, StateMachine);
        huntState = new HuntState(this, StateMachine);
        searchingState = new SearchingState(this, StateMachine);
        baladeState = new BaladeState(this, StateMachine);
    }

    private void Update()
    {
        if(!footstepAudio.isPlaying)
        {
            int actualClip = 5;
            int rdmClip = Random.Range(0, audioClips.Length);
            
            if(actualClip != rdmClip)
            {
                actualClip = rdmClip;
                footstepAudio.clip = audioClips[rdmClip];
                footstepAudio.Play();
            }
        }

        if (playerRef == null)
        {
            if (name == "MonsterCancerServer")
                playerRef = GameObject.Find("Player1");
            else
                playerRef = GameObject.Find("Player2");
        }

        if (!soundDetected)
        {
            SoundDetection();
        }

        StateMachine._CurrentState.FrameUpdate();
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

            Debug.Log("Le son est jou� ?" + sound.audioSource.isPlaying);

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
    public void FindRoomManager()
    {
        if (!inPatrol)
        {
            StartCoroutine(FindRoom());
        }
        else
        {
            StopCoroutine(FindRoom());
        }
    }
    public IEnumerator FindRoom()
    {
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
            Debug.Log("Le monstre patrouille al�atoirement");
            int rdmRoom = Random.Range(0, rooms.Count);
            actualRoom = rdmRoom;
            Debug.Log(actualRoom);
            enemyNavMesh.SetDestination(rooms[actualRoom].transform.position);
        }  
            float distanceToDestination = Vector3.Distance(transform.position, enemyNavMesh.destination);

            if (distanceToDestination < 0.5f)  // Tol�rance de 0.5 unit�s
            {
                isArrived = true;
            }
            else
            {
                isArrived = false;
            }      
        yield return new WaitForSeconds(0);
    }


    public IEnumerator PathwayCountdown()
    {
        //Debug.Log("Debut Patrouille ma gueule");
        while (pathwayCountdown > 0)
        {
            pathwayCountdown--;
            yield return new WaitForSeconds(1);
        }
    }


#endregion
#region Hunt
    
    public void Killing()
    {
        Debug.Log("Je te tue agougagou");

        animator.SetBool("canKill", true);
        StartCoroutine(Kill());
    }

    public IEnumerator Kill()
    {
        yield return new WaitForSeconds(3);
        losingCanva.SetActive(true);
        Time.timeScale = 0;
    }



    public IEnumerator StopHunt()
    {
        yield return new WaitForSeconds(escapeTiming);
        huntState.Change();
    }
}


#endregion