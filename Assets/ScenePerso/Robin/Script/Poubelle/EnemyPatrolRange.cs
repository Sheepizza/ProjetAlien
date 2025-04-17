using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class EnemyPatrolRange : NetworkBehaviour
{
    public AlienMovement alienMovement;
    // Start is called before the first frame update
    void Start()
    {
        if (isServer && isLocalPlayer)
        alienMovement = GameObject.Find("MonsterCancerServer").GetComponent<AlienMovement>();
        else if (isLocalPlayer)
        alienMovement = GameObject.Find("MonsterCancerClient").GetComponent<AlienMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Room" && alienMovement != null)
        {
            alienMovement.rooms.Add(other.gameObject);
        }

    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Room" && alienMovement != null)
        {
            alienMovement.rooms.Remove(other.gameObject);
        }
    }
}
