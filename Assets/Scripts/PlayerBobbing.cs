using JetBrains.Annotations;
using Mirror;
using Mirror.BouncyCastle.Bcpg.Sig;
using Mirror.BouncyCastle.Tls;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Animations;
using Mirror.Examples.Billiards;

public class PlayerBobbing : MonoBehaviour
{
    public PlayerController playerController;

    [Header("Bobbing Settings")]
    public float bobbingAmount = 0.05f; // Amplitude du mouvement
    public float bobbingSpeed;
    public Vector3 camHold; // Position de base de l'objet
    private float timer = 0f;

    void Update()
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
    }

}