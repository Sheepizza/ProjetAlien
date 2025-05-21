using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsActivate : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnActiveChange))]
    public bool IsActive = false;

    [Command(requiresAuthority = false)]
    public void UpdateColor()
    {
        GetComponent<MeshRenderer>().material.color = IsActive ? Color.green : Color.red;
        RPCUpdateColor();
    }

    [ClientRpc]
    public void RPCUpdateColor()
    {
        GetComponent<MeshRenderer>().material.color = IsActive ? Color.green : Color.red;
    }

    void OnActiveChange(bool _oldActive, bool _newActive)
    {
        GetComponent<MeshRenderer>().material.color = _newActive ? Color.green : Color.red;
    }
}
