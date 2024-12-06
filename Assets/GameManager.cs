using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance => instance;

    private Camera playerCamera; // Référence à la caméra du joueur

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetPlayerCamera(Camera camera)
    {
        playerCamera = camera; // Définit la caméra principale
    }

    public Camera GetPlayerCamera()
    {
        return playerCamera; // Retourne la caméra principale
    }
    
}
