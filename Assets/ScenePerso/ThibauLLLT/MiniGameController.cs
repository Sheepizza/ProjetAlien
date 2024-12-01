using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MiniGameController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private RectTransform redRectangle; // Rectangle rouge
    [SerializeField] private RectTransform greenZone; // Zone verte
    [SerializeField] private RectTransform cursor; // Curseur qui se déplace

    [Header("Game Settings")]
    [SerializeField] private float cursorSpeed = 100f; // Vitesse du curseur
    [SerializeField] private AudioSource failureSoundSource; // Audio source pour jouer le son d'échec

    private Vector2 greenZonePosition;
    private bool movingRight = true;
    private Coroutine cursorCoroutine; // Référence à la coroutine du curseur

    void Start()
    {
        if (redRectangle == null || greenZone == null || cursor == null || failureSoundSource == null)
        {
            Debug.LogError("Tous les éléments UI doivent être assignés dans l'inspecteur.");
            return;
        }

        SetupGreenZone();
        StartCursorCoroutine();
    }

    public void OpenMiniGame()
    {
        gameObject.SetActive(true);
        failureSoundSource.gameObject.SetActive(true);
        failureSoundSource.Stop();
        failureSoundSource.gameObject.SetActive(false);

        // Relance la coroutine du curseur si elle est arrêtée
        if (cursorCoroutine == null)
        {
            StartCursorCoroutine();
        }
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy) return; // Ne pas exécuter si le panel est inactif

        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            StopCursor();
            CheckCursorPosition();
        }
    }

    private void StartCursorCoroutine()
    {
        if (cursorCoroutine == null)
        {
            cursorCoroutine = StartCoroutine(MoveCursor());
        }
    }

    private void StopCursor()
    {
        if (cursorCoroutine != null)
        {
            StopCoroutine(cursorCoroutine);
            cursorCoroutine = null; // Réinitialise la référence de la coroutine
        }
    }

    private IEnumerator MoveCursor()
    {
        while (true) // La boucle continue pour faire bouger le curseur indéfiniment
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

        // Vérifie si le curseur est sur la zone verte ou rouge
        if (cursorPosition >= greenStart && cursorPosition <= greenEnd)
        {
            // Réussite
            Debug.Log("Réussi!");
            ClosePanel();
            ActivateObjects();
        }
        else
        {
            // Échec
            Debug.Log("Échec!");
            ClosePanel();
            PlayFailureSound();
        }
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);

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
            Debug.LogError("L'audio source pour le son d'échec n'est pas assignée.");
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
        // Désactive les objets Broken et active les objets Fixed
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
        // Positionne la zone verte à un endroit aléatoire sur le rectangle rouge
        float maxWidth = redRectangle.rect.width;
        float greenWidth = greenZone.rect.width;
        float xPosition = Random.Range(0, maxWidth - greenWidth);
        greenZone.anchoredPosition = new Vector2(xPosition, greenZone.anchoredPosition.y);

        greenZonePosition = greenZone.anchoredPosition;
    }
}