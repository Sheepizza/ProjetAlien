using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class CellManager : NetworkBehaviour
{
    public Camera playerCamera;
    public GameObject heldCellPickUp;

    void Update()
    {
        if (!isLocalPlayer) return;

        if (heldCellPickUp != null && Input.GetKeyDown(KeyCode.R))
        {
            CmdTryPlaceCell();
        }
    }

    [Command]
    void CmdTryPlaceCell()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Cell"))
        {
            RpcPlaceCell(hit.transform.position, hit.transform.rotation);
        }
    }

    [ClientRpc]
    void RpcPlaceCell(Vector3 position, Quaternion rotation)
    {
        if (heldCellPickUp != null)
        {
            heldCellPickUp.transform.position = position;
            heldCellPickUp.transform.rotation = rotation;
            heldCellPickUp.GetComponent<Rigidbody>().isKinematic = true;
            heldCellPickUp = null;
        }
    }
}
