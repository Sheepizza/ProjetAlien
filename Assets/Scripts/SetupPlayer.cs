using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SetupPlayer : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    Camera _mainCam;

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

    private void OnDestroy()
    {
        if (isLocalPlayer)
        {
            _mainCam.gameObject.SetActive(true);
        }
    }
}
