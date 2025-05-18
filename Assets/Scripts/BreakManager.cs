using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakManager : NetworkBehaviour
{
    public BreakDatas DoorBreakDatas;

    Vector3 startPos;

    [SyncVar(hook = "ChangeState")]
    public bool IsBreak = false;

    float _repairTime;
    float _actualRepairTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        _repairTime = DoorBreakDatas.SecondsToRepair;
        GetComponent<Outline>().OutlineWidth = 0;
        GetComponent<Outline>().enabled = false;
    }

    [Command(requiresAuthority = false)]
    public void ChangeState(bool _state)
    {
        IsBreak = _state;
        Debug.Log(gameObject + " " + IsBreak);
        if (!IsBreak)
        {
            //t = 0;
            //CMDOpenDoorOnRepair();
        }
    }


    //Appelé pendant une réparation
    public IEnumerator Repair()
    {
        while (IsBreak)
        {
            Debug.Log(_actualRepairTime);
            yield return new WaitForSeconds(0.1f);
            _actualRepairTime += 0.1f;
            if (_actualRepairTime >= _repairTime) 
            { 
                Debug.Log("changeGOState");
                //GameObject.Find("Player1").GetComponentInChildren<RepairManager>().ChangeGOState(gameObject, false);
                ChangeState(false);
                Debug.Log(gameObject);
            }
        }
    }

    public void ChangeState(bool OldBool, bool NewBool)
    {
        if (NewBool)
        {
            Debug.Log("ToRepair");
            transform.tag = "ToRepair";
            GetComponent<Outline>().OutlineWidth = 8;
            GetComponent<Outline>().enabled = false;
            transform.position = startPos + Vector3.down * 3;
        }
        else
        {
            Debug.Log("Repaired");
            transform.tag = "Untagged";
            GetComponent<Outline>().OutlineWidth = 0; 
            GetComponent<Outline>().enabled = false;
            CMDOpenDoorOnRepair();
            //transform.position = startPos;
        }
    }

    [Command(requiresAuthority = false)]
    void CMDOpenDoorOnRepair()
    {
        RPCOpenDoorOnRepair();
    }

    [ClientRpc]
    void RPCOpenDoorOnRepair()
    {
        StartCoroutine(OpenDoorOnRepair(this.gameObject));
    }

    IEnumerator OpenDoorOnRepair(GameObject _door)
    {
        Debug.Log("J'ouvre" + _door.name);
        Vector3 _startPos = _door.transform.position;
        float _elapsedTime = 0f;
        int _direction = 1;

        while (_elapsedTime < 1f)
        {
            _door.transform.position = Vector3.Lerp(_startPos, _startPos + Vector3.up * 3 * _direction, _elapsedTime / 1);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        _door.transform.position = _startPos + (Vector3.up * 3 * _direction) / 2;
    }

    /*public IEnumerator DoOpen(GameObject _door)
    {
        Debug.Log("J'ouvre" + _door.name);
        Vector3 _startPos = _door.transform.position;
        float _elapsedTime = 0f;
        int _direction = 1;

        while (_elapsedTime < 1f)
        {
            _door.transform.position = Vector3.Lerp(_startPos, _startPos + Vector3.up * 3 * _direction, _elapsedTime / 1);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        _door.transform.position = _startPos + Vector3.up * 3 * _direction;
    }*/
}
