using Mirror;
using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField]
    [SyncVar]
    public GameObject J1;
    [SerializeField]
    [SyncVar]
    public string NameJ1;
    [SerializeField]
    [SyncVar]
    public uint J1Identity;

    [SerializeField]
    [SyncVar]
    public GameObject J2;
    [SerializeField]
    [SyncVar]
    public string NameJ2;
    [SerializeField]
    [SyncVar]
    public uint J2Identity;

    static GameManager instance = null;
    public static GameManager Instance => instance;
    public MinimapCursor minimapCursor;

    [Header("Datas"), SerializeField]
    BreakAtStart BrokenObjects;
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



    [Command(requiresAuthority = false)]
    public void SetName()
    {
        RPCSetName(J1, J2);
        InitGame();
    }

    void InitGame()
    {
        minimapCursor = J1.GetComponentInChildren<MinimapCursor>();
        minimapCursor.SetupMiniMap();
        J1.GetComponentInChildren<LightManager>().CMDUnactiveAllLights();
        J2.GetComponentInChildren<LightManager>().CMDUnactiveAllLights();
        BreakObjects();
    }

    void BreakObjects()
    {
        foreach (var _obj in BrokenObjects.ObjectsToBreakAtStart)
        {
            GameObject.Find("J1D_A").GetComponent<BreakManager>().IsBreak = true;
        }
    }

    [ClientRpc]
    public void RPCSetName(GameObject _j1, GameObject _j2)
    {
        _j1.gameObject.name = "Player1";
        _j2.gameObject.name = "Player2";
    }
}
