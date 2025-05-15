using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;


public class EnemyPatrolRange : NetworkBehaviour
{
    int player = 0;

    // Start is called before the first frame update
    void Update()
    {
        if(player == 0)
        {
            if (isServer)
                player = 1;
            else
                player = 2;
        }
    }

    // Update is called once per frame

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Room" && player == 1)
        {
            GameManager.Instance.J1Rooms.Add(other.gameObject);
            return;
        }
        else if (other.tag == "Room")
        {
            GameManager.Instance.J2Rooms.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Room" && player == 1)
        {
            GameManager.Instance.J1Rooms.Remove(other.gameObject);
            return;
        }
        else if (other.tag == "Room")
        {
            Debug.Log($"{other.gameObject} has been removed");
            GameManager.Instance.J2Rooms.Remove(other.gameObject);
        }
    }
}
