using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField]
    [SyncVar]
    GameObject J1;
    [SerializeField]
    [SyncVar]
    string NameJ1;
    [SerializeField]
    [SyncVar]
    uint J1Identity;

    [SerializeField]
    [SyncVar]
    GameObject J2;
    [SerializeField]
    [SyncVar]
    string NameJ2;
    [SerializeField]
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
        Debug.Log(_identity);
        Debug.Log(J1Identity);
        if(J1Identity == 0)
        {
            J1Identity = _identity;
            Debug.Log(_identity);
            Debug.Log(J1Identity);
            J1 = GameObject.Find("Player1");
            NameJ1 = J1.gameObject.name;
        }
        else
        {
            J2Identity = _identity;
            J2 = GameObject.Find("Player1");
            NameJ2 = J2.gameObject.name;
        }
        Debug.Log(J1Identity);
    }

    public bool FindPlayer(GameObject _gameObject)
    {

        return (_gameObject == J1);
    }
}
