using JetBrains.Annotations;
using Mirror;
using Mirror.BouncyCastle.Tls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float Speed = 5f;
    public float JumpForce = 0.5f;

    Rigidbody _rb;
    CapsuleCollider _cb;
    public GameObject camHold;  
    public GameObject caps;
    public GameObject capsCrouch;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponentInParent<Rigidbody>();
        _cb = GetComponentInParent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _cb.enabled = false;
            capsCrouch.SetActive(true);
            camHold.transform.position += Vector3.up * -0.75f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _cb.enabled = true;
            
            capsCrouch.SetActive(false);
            camHold.transform.position += Vector3.up * 0.75f;
        }
    }

    private void FixedUpdate()
    {
            _rb.velocity = transform.right * Input.GetAxis("Horizontal") * Speed + 
            transform.up * _rb.velocity.y + 
            transform.forward * Input.GetAxis("Vertical") * Speed;
    }
}
