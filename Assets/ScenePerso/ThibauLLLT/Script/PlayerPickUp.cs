using System.Collections;
using System.Collections.Generic;
using Mirror.Examples.Common;
using UnityEngine;
using Mirror;
using TMPro;
using Unity.Burst.CompilerServices;

public class PlayerPickUp : NetworkBehaviour
{
    public float pickUpRange = 2f;
    public Transform handPosition;
    /*private GameObject PickUpText;
    private GameObject PlaceText;*/
    public Camera playerCamera;
    private GameObject pickedUpObject = null;
    private GameObject highlightedObject = null;

    [Header("Tags d'objets ramassables")]
    public string[] pickableTags;

    void Start()
    {
        
    if (playerCamera == null)
    {
        Debug.LogError("La caméra principale (MainCamera) n'a pas été trouvée !");
    }

    //playerCamera = GameManager.Instance.GetPlayerCamera();
    /*PickUpText = UIManager.Instance.PickUpText;
    PlaceText = UIManager.Instance.PlaceText;*/
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        if (pickedUpObject == null && Input.GetKeyDown(KeyCode.E))
        {
            TryPickUp();
        }
        else if (pickedUpObject != null && Input.GetKeyDown(KeyCode.G))
        {
            RpcDropObject();
        }
        else if (pickedUpObject != null && pickedUpObject.CompareTag("CellPickUp") && Input.GetKeyDown(KeyCode.R))
        {
            TryPlaceCell();
        }

        HighlightObject();
    }

    void TryPickUp()
{
    if (playerCamera == null)
    {
        Debug.LogError("playerCamera est null dans TryPickUp !");
        return;
    }

    Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
    RaycastHit hit;
    /*PickUpText.SetActive(false);
    PlaceText.SetActive(false);*/

    if (Physics.Raycast(ray, out hit, pickUpRange))
    {
        GameObject targetObject = hit.collider.gameObject;
        if (IsPickableObject(targetObject) && pickedUpObject == null)
        {
            NetworkIdentity targetIdentity = targetObject.GetComponent<NetworkIdentity>();
            if (targetIdentity != null) CmdPickUp(targetIdentity);
        }
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
            if (targetObject.CompareTag("CellPickUp"))
            {
                Transform parentCell = targetObject.transform.parent; // Trouver la cellule parente
                if (parentCell != null && parentCell.CompareTag("Cell"))
                {
                    foreach (Transform child in parentCell)
                    {
                        if (child.CompareTag("Light")) // Cherche un enfant avec le tag light
                        {
                            child.gameObject.SetActive(false);
                            Transform powerCutAudio = parentCell.transform.Find("PowerCutAudio"); // Cherche le son en enfant de l'energy cell pour le jouer
                            if (powerCutAudio != null)
                            {
                                AudioSource audioSource = powerCutAudio.GetComponent<AudioSource>();
                                if (audioSource != null && !audioSource.isPlaying)
                                {
                                    audioSource.Play();
                                }
                            }
                            break;

                        }
                    }
                }
            }
            targetObject.transform.SetParent(handPosition);
            targetObject.transform.localPosition = Vector3.zero;
            targetObject.transform.localRotation = Quaternion.identity;
            targetObject.GetComponent<Collider>().enabled = false;
            targetObject.GetComponent<Rigidbody>().isKinematic = true;
            pickedUpObject = targetObject;
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
        }
    }

    void TryPlaceCell()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        //PlaceText.SetActive(false);
        if (Physics.Raycast(ray, out RaycastHit hit, pickUpRange) && hit.collider.CompareTag("Cell"))
        {
            CmdPlaceCell(hit.collider.gameObject);
        }
    }

    [Command]
    void CmdPlaceCell(GameObject targetCell)
    {
        RpcPlaceCell(targetCell);
    }

    [ClientRpc]
    void RpcPlaceCell(GameObject targetCell)
    {
        if (pickedUpObject != null && pickedUpObject.CompareTag("CellPickUp"))
        {
            // Place la Cell tenue par le joueur en enfant l'emplacement voulu
            pickedUpObject.transform.position = targetCell.transform.position;
            pickedUpObject.transform.rotation = targetCell.transform.rotation;
            pickedUpObject.transform.SetParent(targetCell.transform);

            // Activer la lumière associée à la cellule
            foreach (Transform child in targetCell.transform)
            {
                if (child.CompareTag("Light"))
                {
                    child.gameObject.SetActive(true);
                    Transform powerOnAudio = targetCell.transform.Find("PowerOnAudio");
                    if (powerOnAudio != null)
                    {
                        AudioSource audioSource = powerOnAudio.GetComponent<AudioSource>();
                        if (audioSource != null) audioSource.Play();
                    }
                    break;
                }
            }
            Rigidbody rb = pickedUpObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
            pickedUpObject.GetComponent<Collider>().enabled = true; // Réactiver le collider
            pickedUpObject = null;
        }
    }

    void HighlightObject()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        /*PickUpText.SetActive(false);
        PlaceText.SetActive(false);*/

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            GameObject targetObject = hit.collider.gameObject;

            // Affiche PlaceText si l'objet visé a le tag "Cell"
            if (targetObject.CompareTag("Cell") && pickedUpObject != null && pickedUpObject.CompareTag("CellPickUp"))
            {
                //PlaceText.SetActive(true);
            }

            if (IsPickableObject(targetObject) && pickedUpObject == null)
            {
                if (highlightedObject != targetObject)
                {
                    if (highlightedObject != null)
                    {
                        Outline oldOutline = highlightedObject.GetComponent<Outline>();
                        if (oldOutline != null)
                        {
                            oldOutline.enabled = false;
                        }
                    }
                    //PickUpText.SetActive(true);
                    
                    Outline outline = targetObject.GetComponent<Outline>();
                    if (outline != null)
                    {
                        outline.enabled = true;
                    }
                    highlightedObject = targetObject;
                }
                else
                {
                    //PickUpText.SetActive(true);  // S'assurer que le texte reste visible tant que l'objet est visé
                }
            }
            else
            {
                if (highlightedObject != null)
                {
                    Outline outline = highlightedObject.GetComponent<Outline>();
                    if (outline != null)
                    {
                        outline.enabled = false;
                    }
                    highlightedObject = null;
                }
            }
        }
        else
        {
            if (highlightedObject != null)
            {
                Outline outline = highlightedObject.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = false;
                }
                highlightedObject = null;
            }
        }
    }

    void DisableOutlineForTag(string tag)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tag))
        {
            Outline outline = obj.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
        }
    }

    bool IsPickableObject(GameObject targetObject)
    {
        foreach (string tag in pickableTags)
        {
            if (targetObject.CompareTag(tag)) return true;
        }
        return false;
    }
}
