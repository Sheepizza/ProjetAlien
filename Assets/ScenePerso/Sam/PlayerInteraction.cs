using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    DoorManager _doorManager;
    LightManager _lightManager;
    CamManager _camManager;
    PickUpManager _pickUpManager;
    RepairManager _repairManager;

    private void Start()
    {
        _doorManager = GetComponent<DoorManager>();
        _lightManager = GetComponent<LightManager>();
        _camManager = GetComponent<CamManager>();
        _pickUpManager = GetComponent<PickUpManager>();
        _repairManager = GetComponent<RepairManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HighlightManager.Instance.CanInteract && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(HighlightManager.Instance.GetObjectName() + "ici");
            switch (HighlightManager.Instance.GetObjectTag())
            {

                case "PickUp":
                    _pickUpManager.PickUpObject(HighlightManager.Instance.GetObjectName());
                    break;

                case "CamButton":
                    Debug.Log(HighlightManager.Instance.GetObjectName());
                    _camManager.ChangeCam(HighlightManager.Instance.GetObjectName());
                    //Tu mets ta fonction de cam manager, t'as acc�s au nom et au tag dans l'highlightmanager, et au ID dans le GameManager (fait une fonction pour r�cup)
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