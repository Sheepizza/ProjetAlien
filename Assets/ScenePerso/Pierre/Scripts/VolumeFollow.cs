using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class VolumeFollow : NetworkBehaviour
{
    public GameObject Player;
    // Start is called before the first frame update
    public override void OnStartClient()
    {
        Player = GameObject.Find("Player1");
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null)
            Player = GameObject.Find ("Player1");

        if (Player != null)
            gameObject.transform.position = Player.transform.position;
    }
}
