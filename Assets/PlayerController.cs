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
    public float CrouchScale = 0.3f;
        Rigidbody _rb;
    CapsuleCollider _cb;
    public GameObject camHold;  
    public GameObject caps;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponentInParent<Rigidbody>();
        _cb = GetComponentInParent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))  //accroupis
        {
            //_rb.transform.localScale = Vector3 (0, 1, 0); 
            camHold.transform.position += Vector3.up * -0.75f;
            Speed = 2f;
            //ajout anim accroupis
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) //debout
        {
            camHold.transform.position += Vector3.up * 0.75f;
            Speed = 5f;
            //ajout anim debout
        }

        //si il veut se relever mais qu'il touche un mur, il reste accroupit.
    }

    private void FixedUpdate()
    {
            _rb.velocity = transform.right * Input.GetAxis("Horizontal") * Speed + 
            transform.up * _rb.velocity.y + 
            transform.forward * Input.GetAxis("Vertical") * Speed;
    }
}
