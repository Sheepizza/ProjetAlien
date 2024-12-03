using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpManager : NetworkBehaviour
{
    public Transform HandPos;
    GameObject _pickedGO;

    public void Update()
    {
        if (_pickedGO != null && Input.GetKeyDown(KeyCode.F))
        {
            CmdDrop();
        }
    }

    public void PickUpObject(string _GOname)
    {
        if (_pickedGO == null && isLocalPlayer)
        {
            CmdPickUp(_GOname);
        }
    }

    [Command]
    void CmdPickUp(string _GOname)
    {
        RPCPickUp(_GOname);
    }

    [ClientRpc]
    void RPCPickUp(string _GOname)
    {
        _pickedGO = GameObject.Find(_GOname);
        _pickedGO.transform.SetParent(HandPos);
        _pickedGO.transform.localPosition = Vector3.zero;
        _pickedGO.transform.localRotation = Quaternion.identity;

        _pickedGO.GetComponent<Collider>().enabled = false;
        _pickedGO.GetComponent<Rigidbody>().isKinematic = true;
    }

    [Command]
    void CmdDrop()
    {
        RPCDrop();
    }

    [ClientRpc]
    void RPCDrop()
    {
        _pickedGO.transform.SetParent(null);
        _pickedGO.GetComponent<Collider>().enabled = true;
        _pickedGO.GetComponent<Rigidbody>().isKinematic = false;
        _pickedGO = null;
    }
}