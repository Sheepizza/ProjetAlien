using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Mirror;

public class MiniJeu : NetworkBehaviour
{
    [Header("UI Elements")]
    private RectTransform redRectangle; 
    private RectTransform cursor;
    private RectTransform greenZone;

    [Header("Game Settings")]
    [SerializeField] private float cursorSpeed = 100f; 
    private AudioSource failureSoundSource; 
    private Vector2 greenZonePosition;
    private bool movingRight = true;
    private Coroutine cursorCoroutine; 
    public float pickUpRange = 2f;
    private Camera playerCamera;
    private GameObject highlightedObject = null;
    private GameObject repairText;
    private GameObject repairMiniGamePanel;
    private PlayerController playerController;
    private PlayerCamera playerCameraScript;


    void Start()
    {
     if (isLocalPlayer)
            {
                playerCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
                playerCameraScript = playerCamera.GetComponent<PlayerCamera>(); 

                GameObject uiManager = GameObject.Find("UiManager");
                if (uiManager != null)
                {
                    Canvas canvas = uiManager.GetComponentInChildren<Canvas>();
                    if (canvas != null)
                    {
                        repairText = canvas.transform.Find("RepairText")?.gameObject;
                        repairMiniGamePanel = canvas.transform.Find("RepairMiniGame")?.gameObject;
                        redRectangle = canvas.transform.Find("Red Rectangle")?.GetComponent<RectTransform>();
                        cursor = canvas.transform.Find("Cursor")?.GetComponent<RectTransform>();

                        greenZone = UIManager.Instance.greenZone;

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

                if (Input.GetKeyDown(KeyCode.E)) // Ouvre mini jeu quand on appuie sur E
                {
                    OpenRepairPanel();
                    SetupGreenZone();
                    StartCursorCoroutine();
                    if (!gameObject.activeInHierarchy) return;

                    if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0))
                    {
                        StopCursor();
                        CheckCursorPosition();
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
            ClosePanel();
        }
    }
    bool IsBrokenObject(GameObject targetObject)
    {
        return targetObject.CompareTag("Broken");
    }
    public void OpenMiniGame()
    {
        gameObject.SetActive(true);
        failureSoundSource.gameObject.SetActive(true);
        failureSoundSource.Stop();
        failureSoundSource.gameObject.SetActive(false);
        if (cursorCoroutine == null)
        {
            StartCursorCoroutine();
        }
    }
    
    private void StartCursorCoroutine()
    {
        if (cursorCoroutine == null)
        {
            Debug.Log("Début mouvement curseur");
            cursorCoroutine = StartCoroutine(MoveCursor());
        }
        else
        {
            Debug.Log("La coroutine tourne déjà");
        }
    }
    private void StopCursor()
    {
        if (cursorCoroutine != null)
        {
            StopCoroutine(cursorCoroutine);
            cursorCoroutine = null; // R�initialise la r�f�rence de la coroutine
        }
    }
    private IEnumerator MoveCursor()
    {
        while (true) // La boucle continue pour faire bouger le curseur ind�finiment
        {
            Vector2 currentPosition = cursor.anchoredPosition;
            if (movingRight)
            {
                currentPosition.x += cursorSpeed * Time.deltaTime;
                if (currentPosition.x >= redRectangle.rect.width)
                {
                    currentPosition.x = redRectangle.rect.width;
                    movingRight = false;
                }
            }
            else
            {
                currentPosition.x -= cursorSpeed * Time.deltaTime;
                if (currentPosition.x <= 0)
                {
                    currentPosition.x = 0;
                    movingRight = true;
                }
            }
            cursor.anchoredPosition = currentPosition;

            yield return null;
        }
    }
    private void CheckCursorPosition()
    {
        float cursorPosition = cursor.anchoredPosition.x;
        float greenStart = greenZonePosition.x;
        float greenEnd = greenStart + greenZone.rect.width;
        if (cursorPosition >= greenStart && cursorPosition <= greenEnd)
        {
            // R�ussite
            Debug.Log("Réussi!");
            ClosePanel();
            ActivateObjects();
        }
        else
        {
            // �chec
            Debug.Log("échec!");
            ClosePanel();
            PlayFailureSound();
        }
    }
    public void ClosePanel()
{
    if (repairMiniGamePanel != null)
    {
        repairMiniGamePanel.SetActive(false);
    }

    // Réactive les contrôles du joueur et de la caméra
    PlayerController playerController = FindObjectOfType<PlayerController>();
    PlayerCamera playerCameraScript = FindObjectOfType<PlayerCamera>();
    
    if (playerController != null)
    {
        playerController.enabled = true;
    }
    if (playerCameraScript != null)
    {
        playerCameraScript.SetCameraMovement(true);
    }
}
    void OpenRepairPanel()
    {
        if (repairMiniGamePanel != null)
        {
            repairMiniGamePanel.SetActive(true);

            // D�sactive les contr�les du joueur et de la cam�ra
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
    private void PlayFailureSound()
    {
        if (failureSoundSource != null)
        {
            // Active l'AudioSource, joue le son, et le désactive après 2 secondes
            failureSoundSource.gameObject.SetActive(true);
            failureSoundSource.Play();
            StartCoroutine(DisableAudioSourceAfterDelay(2f));
        }
        else
        {
            Debug.LogError("Pas d'audiosource assignée");
        }
    }
    private IEnumerator DisableAudioSourceAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (failureSoundSource != null)
        {
            failureSoundSource.gameObject.SetActive(false);
        }
    }
    private void ActivateObjects()
    {
        // Désactive les objets Broken et active les objets Fixed autour du joueur
        GameObject[] brokenObjects = GameObject.FindGameObjectsWithTag("Broken");
        foreach (GameObject obj in brokenObjects)
        {
            obj.SetActive(false);
        }

        GameObject[] fixedObjects = GameObject.FindGameObjectsWithTag("Fixed");
        foreach (GameObject obj in fixedObjects)
        {
            obj.SetActive(true);
        }
    }
    private void SetupGreenZone()
    {
        // Positionne la zone verte à un endroit random sur le rectangle rouge
        float maxWidth = redRectangle.rect.width;
        float greenWidth = greenZone.rect.width;
        float xPosition = Random.Range(0, maxWidth - greenWidth);
        greenZone.anchoredPosition = new Vector2(xPosition, greenZone.anchoredPosition.y);

        greenZonePosition = greenZone.anchoredPosition;
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
}
    




