using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakManager : NetworkBehaviour
{
    public BreakDatas DoorBreakDatas;

    [SyncVar(hook = "ChangeState")]
    public bool IsBreak = false;

    float _repairTime;
    float _actualRepairTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _repairTime = DoorBreakDatas.SecondsToRepair;
        IsBreak = false;
    }

    public void ChangeState(bool _state)
    {
        IsBreak = _state;
        Debug.Log(gameObject + " " + IsBreak);
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
                GameObject.Find("Player1").GetComponentInChildren<RepairManager>().ChangeGOState(gameObject, false);
                Debug.Log(gameObject);
            }
        }
    }

    public void ChangeState(bool OldBool, bool NewBool)
    {
        Debug.Log("RPC");

        if (NewBool)
        {
            transform.tag = "ToRepair";
            GetComponent<Outline>().OutlineWidth = 8;
            GetComponent<Outline>().enabled = false;
        }
        else
        {
            transform.tag = "Untagged";
            GetComponent<Outline>().OutlineWidth = 0; 
            GetComponent<Outline>().enabled = false;
        }
    }
}
