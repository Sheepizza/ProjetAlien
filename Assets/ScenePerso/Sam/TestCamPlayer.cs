using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestCamPlayer : MonoBehaviour
{
    [SerializeField]
    GameObject cam;
    [SerializeField]
    GameObject camHolder;

    [SerializeField]
    float sensitivity;

    float x, y;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        cam.transform.position = camHolder.transform.position;

        x += Input.GetAxis("Mouse X") * (sensitivity * Time.deltaTime);
        y -= Input.GetAxis("Mouse Y") * (sensitivity * Time.deltaTime);

        cam.transform.localRotation = Quaternion.Euler(Mathf.Clamp(y, -60, 70), x, 0);
        transform.localRotation = Quaternion.Euler(0, x, 0);
    }
}
