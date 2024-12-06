using System.Collections;
using System.Collections.Generic;
using Mirror.Examples.Common;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;


public class RepairPanel : NetworkBehaviour
{
    public float pickUpRange = 2f;
    public string[] brokenTags;
    private Camera playerCamera;
    private GameObject highlightedObject = null;
    private GameObject repairText;
    private GameObject repairMiniGamePanel;
    private PlayerController playerController;
    private PlayerCamera playerCameraScript;
    public MiniGameController miniGame;

    void Start()
    {
        if (isLocalPlayer)
        {
            playerCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
            playerCameraScript = playerCamera.GetComponent<PlayerCamera>(); 

            GameObject uiManager = GameObject.Find("UIManager");
            if (uiManager != null)
            {
                Canvas canvas = uiManager.GetComponentInChildren<Canvas>();
                if (canvas != null)
                {
                    repairText = canvas.transform.Find("RepairText")?.gameObject;
                    repairMiniGamePanel = canvas.transform.Find("RepairMiniGame")?.gameObject;
                    

                    if (repairText != null)
                    {
                        repairText.SetActive(false);
                    }

                    if (repairMiniGamePanel != null)
                    {
                        repairMiniGamePanel.SetActive(false);
                    }
                }
            }

            playerController = GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            GameObject targetObject = hit.collider.gameObject;
            if (IsBrokenObject(targetObject))
            {
                if (repairText != null) repairText.SetActive(true);

                HighlightObject(targetObject);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    OpenRepairPanel();
                }
            }
            else
            {
                if (repairText != null) repairText.SetActive(false);
                if (highlightedObject != null)
                {
                    var outlineComponent = highlightedObject.GetComponent<Outline>();
                    if (outlineComponent != null)
                    {
                        outlineComponent.enabled = false;
                    }
                    highlightedObject = null;
                }
            }
        }
        else
        {
            if (repairText != null) repairText.SetActive(false);
            if (highlightedObject != null)
            {
                var outlineComponent = highlightedObject.GetComponent<Outline>();
                if (outlineComponent != null)
                {
                    outlineComponent.enabled = false;
                }
                highlightedObject = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseRepairPanel();
        }
    }

    void HighlightObject(GameObject targetObject)
    {
        if (highlightedObject != targetObject)
        {
            if (highlightedObject != null)
            {
                var previousOutline = highlightedObject.GetComponent<Outline>();
                if (previousOutline != null)
                {
                    previousOutline.enabled = false;
                }
            }

            var currentOutline = targetObject.GetComponent<Outline>();
            if (currentOutline != null)
            {
                currentOutline.enabled = true;
            }
            highlightedObject = targetObject;
        }
    }

    bool IsBrokenObject(GameObject targetObject)
{
    return targetObject.CompareTag("Broken");
}

    public void OpenRepairPanel()
    {
        if (!repairMiniGamePanel.activeSelf)
        {
            repairMiniGamePanel.SetActive(true);
            
            //désactive caméra
            if (playerController != null)
            {
                playerController.enabled = false;
            }
            if (playerCameraScript != null)
            {
                playerCameraScript.SetCameraMovement(false);
            }
        }
    }

    public void CloseRepairPanel()
    {
        if (repairMiniGamePanel.activeSelf)
        {
            repairMiniGamePanel.SetActive(false);

            if (playerController != null)
            {
                playerController.enabled = true;
            }
            if (playerCameraScript != null)
            {
                playerCameraScript.SetCameraMovement(true);
            }
        }
    }
}