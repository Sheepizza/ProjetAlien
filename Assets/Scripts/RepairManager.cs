using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairManager : NetworkBehaviour
{
    BreakManager _GObreakManager;

    public string RepairGOName = "RepairGun";
    public GameObject Hand;

    bool _canRepair = false;
    bool _actualRepair = false;

    GameObject _GOToChangeState;

    GameObject porteC;
    GameObject porteA;

    void Update()
    {
        //Vérifie si le joueur a l'outil de réparation dans sa main, si oui, _canRepair = true
        if (Hand.transform.childCount != 0)
        {
            if (Hand.transform.GetChild(0).name == "RepairGun" && !_canRepair)
            {
                _canRepair = true;
            }
            else if (Hand.transform.GetChild(0).name != "RepairGun" && _canRepair)
            {
                _canRepair = false;
            }
        }
        else if (_canRepair)
        {
            _canRepair = false;
        }

        //Vérifie si le joueur peut réparer et si l'objet qu'il regarde possède le tag ToRepair
        //Si oui, si le joueur appuie sur E, lance la Coroutine de l'objet cassé Repair pour le réparer
        //Si le joueur enlève le clique du E ou si il ne regarde plus l'objet, la coroutine se stoppe
        if (_canRepair && HighlightManager.Instance.CanInteract)
        {
            if (HighlightManager.Instance.GetObjectTag() == "ToRepair" && GameObject.Find(HighlightManager.Instance.GetObjectName()).GetComponent<BreakManager>())
            {
                _GObreakManager = GameObject.Find(HighlightManager.Instance.GetObjectName()).GetComponent<BreakManager>();
                if (Input.GetKeyDown(KeyCode.E) && !_actualRepair)
                {
                    _GObreakManager.StartCoroutine(nameof(_GObreakManager.Repair));
                    _actualRepair = true;
                }
                else if (Input.GetKeyUp(KeyCode.E) && _actualRepair)
                { 
                    _GObreakManager.StopAllCoroutines();
                    _actualRepair = false;
                }
            }
        }
        else if (_GObreakManager != null)
        {
            _GObreakManager.StopAllCoroutines();
            _GObreakManager = null;
            _actualRepair = false;
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            ChangeGOState(GameObject.Find("A"), true);
        }
    }

    public void ChangeGOState(GameObject _go, bool _state)
    {
        if (isServer)
        {
            _GOToChangeState = _go;
            CmdBreak(_state);
        }
    }

    [Command]
    void CmdBreak(bool _state)
    {
        _GOToChangeState.GetComponent<BreakManager>().ChangeState(_state);
    }
}
