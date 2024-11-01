using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    DoorManager _doorManager;
    CamManager _camManager;

    private void Start()
    {
        _doorManager = GetComponent<DoorManager>();
        _camManager = GetComponent<CamManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HighlightManager.Instance.CanInteract && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(HighlightManager.Instance.GetObjectName() + "ici");
            switch (HighlightManager.Instance.GetObjectTag())
            {

                case "InteractiveObject":
                    break;

                case "CamButton":
                    Debug.Log(HighlightManager.Instance.GetObjectName());
                    _camManager.ChangeCam(HighlightManager.Instance.GetObjectName());
                    //Tu mets ta fonction de cam manager, t'as acc�s au nom et au tag dans l'highlightmanager, et au ID dans le GameManager (fait une fonction pour r�cup)
                    break;

                case "DoorButton":
                    Debug.Log(HighlightManager.Instance.GetObjectName());
                    _doorManager.ChangeDoorState(HighlightManager.Instance.GetObjectName());
                    break;

                case "VentButton":
                    break;

                default:
                    break;
            }
        }
    }
}