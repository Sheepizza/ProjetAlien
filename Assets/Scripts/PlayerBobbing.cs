using System.Collections;
using UnityEngine;

public class PlayerBobbing : MonoBehaviour
{
    [Header("Bobbing Settings")]
    public float bobbingAmount = 0.05f; // Amplitude du mouvement
    public float bobbingSpeed;
    public Vector3 camHold; // Position de base de l'objet
    private float timer = 0f;

    private void Start()
    {
        InputManager.MovePerformedActions += StartBobbing;
    }
    /*void Update()
    {
        // Si le joueur bouge (par exemple, via les touches ZQSD ou flèches)
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            timer += Time.deltaTime * playerController.moveSpeed * bobbingSpeed;

            // Appliquer un mouvement sinusoïdal (oscillation verticale)
            float newY = camHold.y + Mathf.Sin(timer) * bobbingAmount;

            // Mettre à jour la position
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
        else
        {
            // Si le joueur ne bouge pas, on revient à la position de base
            timer = 0f;
            transform.localPosition = new Vector3(transform.localPosition.x, camHold.y, transform.localPosition.z);
        }
    }*/

    public void StartBobbing() => StartCoroutine(Bobbing());

    IEnumerator Bobbing()
    {
        while (InputManager.Instance.MoveDirection() != Vector2.zero)
        {
            timer += Time.deltaTime /** playerController.moveSpeed */* bobbingSpeed;

            // Appliquer un mouvement sinusoïdal (oscillation verticale)
            float newY = camHold.y + Mathf.Sin(timer) * bobbingAmount;

            // Mettre à jour la position
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);

            yield return null;
        }

        timer = 0f;
        transform.localPosition = new Vector3(transform.localPosition.x, camHold.y, transform.localPosition.z);
        yield break;
    }

}