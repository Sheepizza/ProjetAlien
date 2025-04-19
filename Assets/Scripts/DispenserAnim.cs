using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserAnim : MonoBehaviour
{
    [Header("<size=20><i>Réglages Interaction</i></size>")]
    public float interactionDistance = 3f;
    public KeyCode interactionKey = KeyCode.F;

    [Header(" ")]
    [Header("<size=20><iObjet Distribué</i></size>")]
    public GameObject objectToAnimate;
    public string animationTrigger = "Dispense";

    private bool hasDispensed = false;
    private Camera mainCamera;
    private Animator animator;

    void Start()
    {
        mainCamera = Camera.main;

        if (objectToAnimate != null)
        {
            animator = objectToAnimate.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = false;
            }
            Rigidbody rb = objectToAnimate.GetComponent<Rigidbody>();
            if (rb != null) Destroy(rb);
        }
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
                    StartCoroutine(DispenseObject());
                }
            }
        }
    }

    private IEnumerator DispenseObject()
    {
        hasDispensed = true;
        if (animator != null)
        {
            animator.enabled = true;
            animator.SetTrigger(animationTrigger);
        }
        else
        {
            Debug.LogError("Animator introuvable");
        }
        yield return new WaitForSeconds(1.5f);
        if (objectToAnimate != null && objectToAnimate.GetComponent<Rigidbody>() == null)
        {
            objectToAnimate.AddComponent<Rigidbody>();
            Debug.Log("Rigidbody ajouté");
        }
        else
        {
            Debug.LogWarning("Rigidbody déjà présent ou objet null");
        }

        Debug.Log("Objet lâché");
    }
}
