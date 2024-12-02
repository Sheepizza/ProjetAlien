using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SyncVar]
    GameObject J1;
    [SyncVar]
    uint J1Identity;

    [SyncVar]
    GameObject J2;
    [SyncVar]
    uint J2Identity;

    static GameManager instance = null;
    public static GameManager Instance => instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetPlayer(uint _identity)
    {
        if(J1Identity == 0)
        {
            J1Identity = _identity;
            J1 = Utils.GetSpawnedInServerOrClient(J1Identity).gameObject;
        }
        else
        {
            J2Identity = _identity;
            J2 = Utils.GetSpawnedInServerOrClient(J1Identity).gameObject;
        }
        Debug.Log(J1Identity);
    }

    public bool FindPlayer(GameObject _gameObject)
    {

        return (_gameObject == J1);
    }
}
