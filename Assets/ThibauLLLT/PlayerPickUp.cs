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
    public TMP_Text handFullText;
    public TMP_Text pickUpPromptText;
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
            if (handFullText != null) handFullText.enabled = false;
            if (pickUpPromptText != null) pickUpPromptText.gameObject.SetActive(false);
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
            else if (pickedUpObject != null)
            {
                StartCoroutine(ShowHandFullMessage());
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
                    }

                    var outline = targetObject.GetComponent<Outline>();
                    if (outline != null)
                    {
                        outline.enabled = true;
                    }
                    highlightedObject = targetObject;
                }
                if (pickUpPromptText != null) pickUpPromptText.gameObject.SetActive(true);
            }
            else
            {
                if (highlightedObject != null)
                {
                    highlightedObject.GetComponent<Outline>().enabled = false;
                    highlightedObject = null;
                }
                if (pickUpPromptText != null) pickUpPromptText.gameObject.SetActive(false);
            }
        }
        else
        {
            if (highlightedObject != null)
            {
                highlightedObject.GetComponent<Outline>().enabled = false;
                highlightedObject = null;
            }
            if (pickUpPromptText != null) pickUpPromptText.gameObject.SetActive(false);
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
            }
        }
    }
    IEnumerator ShowHandFullMessage()
    {
        if (handFullText != null)
        {
            handFullText.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            handFullText.gameObject.SetActive(false);
        }
    }
}
