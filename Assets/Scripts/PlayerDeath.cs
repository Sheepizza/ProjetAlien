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
using TMPro;

public class PlayerDeath : MonoBehaviour
{
    public PlayerController playerController;
    public PlayerBobbing playerBobbing;
    public CapsuleCollider _bc;
    public Camera camera;
    public GameObject camPosDeath;
    public TextMeshProUGUI textDeath;
    

    void OnTriggerEnter(Collider other)
    {
          if(other.tag == "mort")
          {
            StartCoroutine(CinematicDeath());
          }
    }

    IEnumerator CinematicDeath()
    {
        bool Aie = true;
        while(Aie == true)
        {
            Debug.Log ("Tu meurs");
            camera.transform.position = camPosDeath.transform.position;
            camera.transform.rotation = camPosDeath.transform.rotation;
            textDeath.gameObject.SetActive(true);
            playerController.enabled = false;
            playerBobbing.enabled = false;
            yield return null;
        }
        
    }
}
