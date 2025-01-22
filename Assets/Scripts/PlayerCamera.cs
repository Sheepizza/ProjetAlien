using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : NetworkBehaviour
{
    public float Sensitivity = 350f;
    float X, Y;

    public GameObject Player;
    public GameObject CameraHolder;
    public bool CanMove = true; 

    private void Start()
    {
        GameManager.Instance.SetPlayerCamera(GetComponent<Camera>());
        Cursor.lockState = CursorLockMode.Locked;
        HighlightManager.Instance.DisableOutlineForTag();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            HighlightManager.Instance.HighlightObject(GetComponent<Camera>());
        }
    }

    // FixedUpdate est appel� � un intervalle fixe, utilis� pour les calculs de physique
    void FixedUpdate()
    {
        if (CanMove)
        {
            transform.position = CameraHolder.transform.position;

            X += Input.GetAxis("Mouse X") * (Sensitivity * Time.deltaTime);
            Y -= Input.GetAxis("Mouse Y") * (Sensitivity * Time.deltaTime);

            transform.rotation = Quaternion.Euler(Mathf.Clamp(Y, -60, 70), X, 0);
            Player.transform.rotation = Quaternion.Euler(0, X, 0);
        }
    }

    public void SetCameraMovement(bool enable)
    {
        CanMove = enable;
    }
}
