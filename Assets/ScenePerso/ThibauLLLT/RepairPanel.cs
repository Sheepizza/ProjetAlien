using System.Collections;
using System.Collections.Generic;
using Mirror.Examples.Common;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;


public class RepairPanel : NetworkBehaviour
{
    public GameObject repairText; // Référence au texte de réparation
    public GameObject repairMiniGamePanel; // Référence au panel de mini-jeu

    private Camera playerCamera; // Référence à la caméra du joueur
    private PlayerController playerController; // Référence au contrôleur du joueur
    private PlayerCamera playerCameraScript; // Référence au script de la caméra

    void Start()
    {
        // Vérifie si ce script est attaché à l'objet du joueur local
        if (isLocalPlayer)
        {
            // Trouve la caméra appelée "PlayerCamera" dans le prefab du joueur
            playerCamera = GameObject.Find("PlayerCamera")?.GetComponent<Camera>();
            if (playerCamera != null)
            {
                playerCameraScript = playerCamera.GetComponent<PlayerCamera>();
            }

            playerController = GetComponent<PlayerController>(); // Trouve le contrôleur du joueur dans la scène

            // Désactive les éléments au départ
            if (repairText != null) repairText.SetActive(false);
            if (repairMiniGamePanel != null) repairMiniGamePanel.SetActive(false);
        }
    }

    void Update()
    {
        // Vérifie que le script est pour le joueur local
        if (!isLocalPlayer) return;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        // Si l'objet est visé par le rayon, on active le texte
        if (Physics.Raycast(ray, out hit, 10f))
        {
            GameObject targetObject = hit.collider.gameObject;
            if (targetObject == this.gameObject) // Vérifie que l'objet visé est le bon
            {
                if (repairText != null) repairText.SetActive(true);

                // Ouvre le panel si la touche 'E' est pressée
                if (Input.GetKeyDown(KeyCode.E))
                {
                    CmdOpenRepairPanel();
                }
            }
            else
            {
                // Désactive le texte si l'objet n'est pas visé
                if (repairText != null) repairText.SetActive(false);
            }
        }
        else
        {
            // Désactive le texte si aucun objet n'est visé
            if (repairText != null) repairText.SetActive(false);
        }

        // Ferme le panel si la touche Échap est pressée
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CmdCloseRepairPanel();
        }
    }

    [Command]
    void CmdOpenRepairPanel()
    {
        if (repairMiniGamePanel != null)
        {
            repairMiniGamePanel.SetActive(true);

            // Désactive les contrôles du joueur et de la caméra
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

    [Command]
    public void CmdCloseRepairPanel()
    {
        if (repairMiniGamePanel != null)
        {
            repairMiniGamePanel.SetActive(false);

            // Réactive les contrôles du joueur et de la caméra
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