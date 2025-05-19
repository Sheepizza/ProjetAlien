using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class PickUpManager : NetworkBehaviour
{
    public Transform HandPos;
    public LDObjectManager _LDObjectManager;
    GameObject _pickedGO;
    [Header("Animator"), SerializeField]
    Animator animator;

    List<GameObject> keysInHand = new List<GameObject>();

    [ClientRpc]
    public void SetupLDManager()
    {
        _LDObjectManager = GetComponent<LDObjectManager>();
    }
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

        if (_LDObjectManager.IsKey(_pickedGO))
        {
            keysInHand.Add(_pickedGO);
            _pickedGO = null;
            _pickedGO.SetActive(false);
            return;
        }

        //_LDObjectManager.GiveObjectTag(_pickedGO);
        _pickedGO.transform.SetParent(HandPos);
        _pickedGO.transform.localPosition = Vector3.zero;
        _pickedGO.transform.localRotation = Quaternion.identity;

        _pickedGO.GetComponent<Collider>().enabled = false;
        _pickedGO.GetComponent<Rigidbody>().isKinematic = true;

        animator.SetBool("Holding", true);
    }

    [Command]
    void CmdDrop()
    {
        RPCDrop();
    }

    [ClientRpc]
    void RPCDrop()
    {
        if(_pickedGO)
        {
            _pickedGO.transform.SetParent(null);
            //_LDObjectManager.DropObject();
            _pickedGO.GetComponent<Collider>().enabled = true;
            _pickedGO.GetComponent<Rigidbody>().isKinematic = false;
            _pickedGO = null;
        }

        animator.SetBool("Holding", false);
    }
}