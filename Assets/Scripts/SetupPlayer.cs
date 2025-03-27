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

        if (isServer && isLocalPlayer)
        {
            transform.parent.gameObject.name = "Player1";
            J1PlayerIdentity = transform.parent.gameObject.GetComponent<NetworkIdentity>().assetId;
            GameManager.Instance.SetPlayer(J1PlayerIdentity);
        }
        else if (!isServer && isLocalPlayer)
        {
            transform.parent.gameObject.name = "Player2";
            J2PlayerIdentity = transform.parent.gameObject.GetComponent<NetworkIdentity>().assetId;
            GameManager.Instance.SetPlayer(J2PlayerIdentity);
        }
    
    }

    /*private void OnDestroy()
    {
        if (isLocalPlayer)
        {
            _mainCam.gameObject.SetActive(true);
        }
    }*/
}
