using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Mirror;


public class MiniGameController : NetworkBehaviour
{
    [Header("UI Elements")]
    private RectTransform redRectangle;
    private RectTransform greenZone; 
    private RectTransform cursor; // Curseur qui se déplace

    [Header("Game Settings")]
    [SerializeField] private float cursorSpeed = 100f; 
    [SerializeField] private AudioSource failureSoundSource;

    private Vector2 greenZonePosition;
    private bool movingRight = true;

    void Start()
    {
        greenZone = UIManager.Instance.greenZone;
        redRectangle = UIManager.Instance.redRectangle;
        cursor = UIManager.Instance.cursor;
        SetupGreenZone();
        StartCursorCoroutine(false);
    }

    // public void OpenMiniGame()
    // {
    //     gameObject.SetActive(true);
    //     failureSoundSource.gameObject.SetActive(true);
    //     failureSoundSource.Stop();
    //     failureSoundSource.gameObject.SetActive(false);
    // }

    void Update()
    {

        if (!gameObject.activeInHierarchy) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            
            // StopCursor();
            CheckCursorPosition();
        }
    }

    public void StartCursorCoroutine(bool cursorCoroutine)
    {
         if (cursorCoroutine == false)
         {
            cursorCoroutine = true;
            StartCoroutine(MoveCursor());
            
         }
        else
         {
            cursorCoroutine = false;
            StopCoroutine(MoveCursor());
            
         }
    }

    void OnEnable()
    {
        StartCursorCoroutine(false);
        SetupGreenZone();
    }


    void OnDisable()
    {
        StartCursorCoroutine(true);
    }

    // private void StopCursor()
    // {
    //     if (cursorCoroutine == true)
    //     {
    //         StopCoroutine(MoveCursor());
    //         cursorCoroutine = false;
    //     }
    // }

    public IEnumerator MoveCursor()
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
        if (cursorPosition >= greenStart && cursorPosition <= greenEnd) // Vérifie si le curseur est sur le rectangle vert. Si c'est le cas, réussi.
        {
            Debug.Log("Réussi!");
            ClosePanel();
            ActivateObjects();
        }
        else
        {
            Debug.Log("échec!");
            ClosePanel();
            PlayFailureSound();
        }
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
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
        failureSoundSource.gameObject.SetActive(true);
        failureSoundSource.Play();
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
        float maxWidth = redRectangle.rect.width;
        float greenWidth = greenZone.rect.width;
        float xPosition = Random.Range(0, maxWidth - greenWidth);
        greenZone.anchoredPosition = new Vector2(xPosition, greenZone.anchoredPosition.y); //Met la Zone verte en random sur la rouge

        greenZonePosition = greenZone.anchoredPosition;
    }
}