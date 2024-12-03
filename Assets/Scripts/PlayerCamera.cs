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

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        HighlightManager.Instance.DisableOutlineForTag();
    }
    // Start is called before the first frame update
    void Update()
    {
        HighlightManager.Instance.HighlightObject(GetComponent<Camera>());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = CameraHolder.transform.position;

        X += Input.GetAxis("Mouse X") * (Sensitivity * Time.deltaTime);
        Y -= Input.GetAxis("Mouse Y") * (Sensitivity * Time.deltaTime);

        transform.rotation = Quaternion.Euler(Math.Clamp(Y,-60,70), X, 0);
        Player.transform.rotation = Quaternion.Euler(0, X, 0);
    }
}
