using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsActivate : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnActiveChange))]
    public bool IsActive = false;

    void OnActiveChange(bool _oldActive, bool _newActive)
    {
        GetComponent<MeshRenderer>().material.color = _newActive ? Color.green : Color.red;
    }
}
