using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolRange : MonoBehaviour
{
    public EnemyPathway enemyPathway;
    // Start is called before the first frame update
    void Start()
    {
        enemyPathway = GameObject.Find("MonsterCancer").GetComponent<EnemyPathway>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Room")
        {
            Debug.Log("Room Detected");
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
