using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsActivate : NetworkBehaviour
{
    [SyncVar]
    public bool IsActive = false;
}
