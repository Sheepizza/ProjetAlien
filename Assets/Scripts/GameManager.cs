using Mirror;
using System.Collections.Generic;
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
    public List<GameObject> J1Rooms;

    [SerializeField]
    [SyncVar]
    public GameObject J2;
    [SerializeField]
    [SyncVar]
    public string NameJ2;
    [SerializeField]
    [SyncVar]
    public uint J2Identity;
    [SyncVar]
    public List<GameObject> J2Rooms;

    [SyncVar]
    public List<string> Keys;

    static GameManager instance = null;
    public static GameManager Instance => instance;
    public MinimapCursor minimapCursor;
    public AmbiantSoundsManager ambiantSoundsManager;

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
        J1.GetComponentInChildren<PickUpManager>().SetupLDManager();
        J2.GetComponentInChildren<PickUpManager>().SetupLDManager();
        BreakObjects();
        GoAmbiantSound();
    }

    void BreakObjects()
    {
        foreach (var _obj in BrokenObjects.ObjectsToBreakAtStart)
        {
            GameObject.Find(_obj).GetComponent<BreakManager>().IsBreak = true;
        }
    }

    [ClientRpc]
    public void RPCSetName(GameObject _j1, GameObject _j2)
    {
        _j1.gameObject.name = "Player1";
        _j2.gameObject.name = "Player2";
    }

    [Command(requiresAuthority = false)]
    void GoAmbiantSound()
    {
        RpcGoAmbiantSound();
    }

    [ClientRpc]
    void RpcGoAmbiantSound()
    {
        ambiantSoundsManager.goPlaySound = true;
    }

    public void AddKey(string _key)
    {
        Keys.Add(_key);
    }
}
