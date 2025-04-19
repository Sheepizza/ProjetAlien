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

            // On désactive l’Animator au lancement pour éviter tout lancement automatique
            if (animator != null)
            {
                animator.enabled = false;
            }

            // On supprime un Rigidbody si jamais il existe déjà au démarrage
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
            animator.enabled = true; // ✅ On active l'Animator
            animator.SetTrigger(animationTrigger); // 🔥 On déclenche l'anim
        }
        else
        {
            Debug.LogError("Animator introuvable !");
        }

        // ⏳ Attend que l’animation se termine (ajuste la durée si besoin)
        yield return new WaitForSeconds(1.5f);

        // ✅ Ajoute le Rigidbody pour faire tomber l’objet
        if (objectToAnimate != null && objectToAnimate.GetComponent<Rigidbody>() == null)
        {
            objectToAnimate.AddComponent<Rigidbody>();
            Debug.Log("✅ Rigidbody ajouté à " + objectToAnimate.name);
        }
        else
        {
            Debug.LogWarning("Rigidbody déjà présent ou objet null");
        }

        Debug.Log("Objet distribué !");
    }
}
