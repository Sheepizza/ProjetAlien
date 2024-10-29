using System.Collections;
using System.Collections.Generic;
using Mirror.Examples.Common;
using UnityEngine;
using Mirror;
using TMPro;

public class PlayerPickUp : NetworkBehaviour
{
    public float pickUpRange = 2f;
    public Transform handPosition;
    public GameObject  PickUpText;
    private Camera playerCamera;
    private GameObject pickedUpObject = null;
    private GameObject highlightedObject = null;
    [Header("Tags d'objets ramassables")]
    public string[] pickableTags;
    void Start()
    {
            if (isLocalPlayer)
    {
        playerCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();

        // Trouver PickupText automatiquement
        GameObject uiManager = GameObject.Find("UiManager");
        if (uiManager != null)
        {
            Canvas canvas = uiManager.GetComponentInChildren<Canvas>();
            if (canvas != null)
            {
                PickUpText = canvas.transform.Find("PickUpText")?.gameObject;
            }
        }

        if (PickUpText == null)
        {
            Debug.LogWarning("PickupText non trouvé dans UiManager. Assurez-vous que l'hiérarchie est correcte.");
        }
        else
        {
            PickUpText.SetActive(false); // Initialement caché
        }

        foreach (string tag in pickableTags)
        {
            DisableOutlineForTag(tag);
        }
    }
        }
    void Update()
    {
        if (!isLocalPlayer) return;

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
                RpcDropObject();
            }
        }
        HighlightObject();
        }
        void TryPickUp()
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;
            PickUpText.gameObject.SetActive(false);
            if (Physics.Raycast(ray, out hit, pickUpRange))
            {
                GameObject targetObject = hit.collider.gameObject;
                if (IsPickableObject(targetObject) && pickedUpObject == null)
                {
                    NetworkIdentity targetIdentity = targetObject.GetComponent<NetworkIdentity>();


                    if (targetIdentity != null)
                    {
                        CmdPickUp(targetIdentity);
                    }
                }
            }
        }
        bool IsPickableObject(GameObject targetObject)
        {
        foreach (string tag in pickableTags)
        {
            if (targetObject.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
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
            targetObject.transform.SetParent(handPosition);
            targetObject.transform.localPosition = Vector3.zero;
            targetObject.transform.localRotation = Quaternion.identity;

            targetObject.GetComponent<Collider>().enabled = false;
            targetObject.GetComponent<Rigidbody>().isKinematic = true;

            pickedUpObject = targetObject;
            Debug.Log("Objet ramassé et placé dans la main !");
        }
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
            Debug.Log("Objet lâché au sol !");
        }
    }
    void HighlightObject()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            GameObject targetObject = hit.collider.gameObject;
            if (IsPickableObject(targetObject) && pickedUpObject == null)
            {
                if (highlightedObject != targetObject)
                {
                    if (highlightedObject != null)
                    {
                        highlightedObject.GetComponent<Outline>().enabled = false;
                        PickUpText.gameObject.SetActive(false);
                    }
                    PickUpText.gameObject.SetActive(true);
                    var outline = targetObject.GetComponent<Outline>();
                    if (outline != null)
                    {
                        outline.enabled = true;
                    }
                    highlightedObject = targetObject;
                }
            }
            else
            {
                if (highlightedObject != null)
                {
                    highlightedObject.GetComponent<Outline>().enabled = false;
                    PickUpText.gameObject.SetActive(false);
                    highlightedObject = null;
                }
            }
        }
        else
        {
            if (highlightedObject != null)
            {
                highlightedObject.GetComponent<Outline>().enabled = false;
                PickUpText.gameObject.SetActive(false);
                highlightedObject = null;
            }
        }
    }
    void DisableOutlineForTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            var outline = obj.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
                PickUpText.gameObject.SetActive(false);
            }
        }
    }
}
