using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    DoorManager _doorManager;
    LightManager _lightManager;

    private void Start()
    {
        _doorManager = GetComponent<DoorManager>();
        _lightManager = GetComponent<LightManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HighlightManager.Instance.CanInteract && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(HighlightManager.Instance.GetObjectName());
            switch (HighlightManager.Instance.GetObjectTag())
            {

                case "InteractiveObject":
                    break;

                case "CamButton":
                    //Tu mets ta fonction de cam manager, t'as accès au nom et au tag dans l'highlightmanager, et au ID dans le GameManager (fait une fonction pour récup)
                    break;

                case "DoorButton":
                    _doorManager.ChangeDoorState(HighlightManager.Instance.GetObjectName());
                    break;

                case "VentButton":
                    break;

                case "LightButton":
                    _lightManager.ChangeLightState(HighlightManager.Instance.GetObjectName());
                    break;

                default:
                    break;
            }
        }
    }
}