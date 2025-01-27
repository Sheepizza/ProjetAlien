using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class EnemyPatrolRange : NetworkBehaviour
{
    public AIManager manager;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject == isServer && gameObject == isLocalPlayer)
        manager = GameObject.Find("MonsterCancerServer").GetComponent<AIManager>();
        else if (gameObject == isLocalPlayer)
        manager = GameObject.Find("MonsterCancerClient").GetComponent<AIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Room")
        {
            manager.pathways.Add(other.gameObject);
        }

    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Room")
        {
            manager.pathways.Remove(other.gameObject);
        }
    }
}
