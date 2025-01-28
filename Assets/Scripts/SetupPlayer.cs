using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SetupPlayer : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;
    
    Camera _mainCam;

    [SerializeField]
    uint J1PlayerIdentity;

    [SerializeField]
    uint J2PlayerIdentity;

    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            foreach (var component in componentsToDisable)
            {
                component.enabled = false;
            }
        }
        else
        {
            _mainCam = Camera.main;
            if (_mainCam != null)
            {
                _mainCam.gameObject.SetActive(false);
            }
        }    
    }

    /*private void OnDestroy()
    {
        if (isLocalPlayer)
        {
            _mainCam.gameObject.SetActive(true);
        }
    }*/

    public override void OnStartClient()
    {
        
        if(isClient && !isServer)
        {
            Debug.Log("BonjourClient");
            SendInfoClient();
        }
        if(isServer && isClient)
        {
            Debug.Log("BonjourServer");
            SendInfoServer();
            //Debug.Log(transform.parent.gameObject.GetComponent<NetworkIdentity>().netId);
        }
    }

    [Command]
    public void SendInfoClient()
    {
        GameManager.Instance.J2Identity = transform.parent.gameObject.GetComponent<NetworkIdentity>().netId;
        GameManager.Instance.NameJ2 = "Player2";
        GameManager.Instance.J2 = transform.parent.gameObject;
        GameManager.Instance.SetName();
    }

    [Command]
    public void SendInfoServer()
    {
        GameManager.Instance.NameJ1 = "Player1";
        GameManager.Instance.J1Identity = transform.parent.gameObject.GetComponent<NetworkIdentity>().netId;
        GameManager.Instance.J1 = transform.parent.gameObject;
    }
}
