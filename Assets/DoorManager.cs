using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DoorManager : NetworkBehaviour
{
    public GameObject Door;

    void Start()
    {
        Door = GameObject.Find("Door");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Door != null)
        {
            CmdToggleDoor(Door.activeSelf);
        }
    }

    [Command]
    public void CmdToggleDoor(bool doorState)
    {
        Door.SetActive(!doorState);
        RPCToggleDoor(doorState);
    }

    [ClientRpc]
    public void RPCToggleDoor(bool doorState)
    {
        Door.SetActive(!doorState);
    }
}
