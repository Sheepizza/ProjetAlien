using System.Collections;
using System.Collections.Generic;
using Mirror.Examples.Common;
using UnityEngine;
using Mirror;

public class PlayerPickUp : NetworkBehaviour
{
    public float pickUpRange = 3f;
    public Transform handPosition;
    private Camera playerCamera;
    private GameObject highlightedObject = null;

    private GameObject pickedUpObject = null;

    void Start()
    {
            if (isLocalPlayer)
            {
                playerCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
            }
            GameObject[] pickUpObjects = GameObject.FindGameObjectsWithTag("PickUp");
        foreach (GameObject obj in pickUpObjects)
        {
            var outline = obj.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
        }
    }

    void Update()
    {
        if (!isLocalPlayer) return;
        HighlightObject();
        if (pickedUpObject == null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                TryPickUp();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                CmdDropObject();
            }
        }
    }

    void TryPickUp()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            GameObject targetObject = hit.collider.gameObject;
            if (targetObject.CompareTag("PickUp"))
            {
                Debug.Log("Objet détecté : " + targetObject.name);
                NetworkIdentity targetIdentity = targetObject.GetComponent<NetworkIdentity>();

                if (targetIdentity != null)
                {
                    CmdPickUp(targetIdentity);
                }
                else
                {
                    Debug.Log("Erreur : l'objet n'a pas de NetworkIdentity.");
                }
            }
            else
            {
                Debug.Log("Aucun objet avec le tag 'PickUp' détecté.");
            }
        }
        else
        {
            Debug.Log("Aucun objet détecté.");
        }
    }

    [Command]
    void CmdPickUp(NetworkIdentity targetIdentity)
    {
        RpcPickUp(targetIdentity);
    }
    [ClientRpc]
    void RpcPickUp(NetworkIdentity targetIdentity)
    {
        GameObject targetObject = targetIdentity.gameObject;
        if (targetObject != null)
        {
            targetObject.transform.position = handPosition.position;
            targetObject.transform.rotation = handPosition.rotation;
            targetObject.transform.SetParent(handPosition); 
            targetObject.GetComponent<Collider>().enabled = false;
            targetObject.GetComponent<Rigidbody>().isKinematic = true;

            pickedUpObject = targetObject; 
            Debug.Log("Objet ramassé");
        }
        else
        {
            Debug.Log("Erreur");
        }
    }
    void CmdDropObject()
    {
        RpcDropObject();
    }
    [ClientRpc]
    void RpcDropObject()
    {
        if (pickedUpObject != null)
        {
            pickedUpObject.transform.SetParent(null);
            pickedUpObject.GetComponent<Collider>().enabled = true;
            pickedUpObject.GetComponent<Rigidbody>().isKinematic = false;
            pickedUpObject.transform.position = handPosition.position + handPosition.forward * 0.5f;
            pickedUpObject = null;
            Debug.Log("Objet lâché");
        }
    }
    void HighlightObject()
    {

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            GameObject targetObject = hit.collider.gameObject;
            if (targetObject.CompareTag("PickUp"))
            {
                if (highlightedObject != targetObject)
                {
                    if (highlightedObject != null)
                    {
                        highlightedObject.GetComponent<Outline>().enabled = false;
                    }
                    var outline = targetObject.GetComponent<Outline>();
                    if (outline != null)
                    {
                        outline.enabled = true;
                    }
                    highlightedObject = targetObject;
                }
            }
            else if (highlightedObject != null)
            {
                highlightedObject.GetComponent<Outline>().enabled = false;
                highlightedObject = null;
            }
        }
        else if (highlightedObject != null)
        {
            highlightedObject.GetComponent<Outline>().enabled = false;
            highlightedObject = null;
        }
    }

}
