using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class EnemyPatrolRange : NetworkBehaviour
{
    public EnemyPathway enemyPathway;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject == isServer && gameObject == isLocalPlayer)
        enemyPathway = GameObject.Find("MonsterCancerServer").GetComponent<EnemyPathway>();
        else if (gameObject == isLocalPlayer)
        enemyPathway = GameObject.Find("MonsterCancerClient").GetComponent<EnemyPathway>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Room")
        {
            enemyPathway.pathways.Add(other.gameObject);
        }

    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Room")
        {
            enemyPathway.pathways.Remove(other.gameObject);
        }
    }
}
