using JetBrains.Annotations;
using Mirror;
using Mirror.BouncyCastle.Bcpg.Sig;
using Mirror.BouncyCastle.Tls;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : NetworkBehaviour
{
    public float t;
    public float Speed = 5f;
    Rigidbody _rb;
    CapsuleCollider _cb;
    public CapsuleCollider _cbCrouch;
    public GameObject camCrouch;
    public GameObject camHold;
    Vector3 camStandPosition;
    float colliderStandHeight;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponentInParent<Rigidbody>();
        _cb = GetComponentInParent<CapsuleCollider>();
        camStandPosition = camHold.transform.localPosition;
        colliderStandHeight = _cb.height;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))  //accroupis
        {
            camHold.transform.localPosition = Vector3.Lerp(camHold.transform.localPosition, camCrouch.transform.localPosition, t); //progressivement
            _cb.height = _cbCrouch.height; //progressivement
            Speed = 2f;
            //ajout anim accroupis
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) //debout
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.up, out hit, 2f)) //si il veutse relever mais qu'il touche un mur, il reste accroupit.
            {
                camHold.transform.localPosition = camCrouch.transform.localPosition;
                _cb.height = _cbCrouch.height;
            }
            else
            {
                camHold.transform.localPosition = Vector3.Lerp(camHold.transform.localPosition, camStandPosition, t);
                _cb.height = colliderStandHeight;
                Speed = 5f;
                //ajout anim debout
            }
        }

    }

    private void FixedUpdate()
    {
        _rb.velocity = transform.right * Input.GetAxis("Horizontal") * Speed +
        transform.up * _rb.velocity.y +
        transform.forward * Input.GetAxis("Vertical") * Speed;
    }
    private void waitForSeconds(float seconds)
    {

    }
}