using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLerpTiboL : MonoBehaviour
{
    [Header("<size=20><i>Réglages interaction<i><size>")]
    public float interactionDistance = 3f;
    public KeyCode interactionKey = KeyCode.F;

    [Header("<size=20><i>Objet distribué<i><size>")]
    public GameObject objectToAnimate;
    public Transform targetPosition; // Ajoute un Empty dans la scène pour définir la position finale
    public float moveDuration = 1.2f;

    private bool hasDispensed = false;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (hasDispensed) return;

        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.CompareTag("DispenserButton"))
            {
                if (Input.GetKeyDown(interactionKey))
                {
                    StartCoroutine(MoveAndDispense());
                }
            }
        }
    }

    private IEnumerator MoveAndDispense()
    {
        hasDispensed = true;

        Vector3 startPos = objectToAnimate.transform.position;
        Vector3 endPos = targetPosition.position;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            objectToAnimate.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        Debug.Log("Déplacement terminé");

        if (objectToAnimate.GetComponent<Rigidbody>() == null)
        {
            objectToAnimate.AddComponent<Rigidbody>();
            Debug.Log("Rigidbody ajouté");
        }
        else
        {
            Debug.Log("Rigidbody déjà présent");
        }
    }
}
