using Mirror;
using Mirror.BouncyCastle.Tls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float Speed = 5f;
    public float JumpForce = 4f;
    public float CrouchHeight = 1f;
    public float StandHeight = 2f;
    public Vector3 StandScale = new Vector3(1f, 1f, 1f);
    public Vector3 CrouchScale = new Vector3(1f, 0.5f, 1f);
    Rigidbody _rb;
    CapsuleCollider _cb;
    Transform transPlayer;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponentInParent<Rigidbody>();
        _cb = GetComponentInParent<CapsuleCollider> ();
        transPlayer = GetComponentInParent<Transform> ();
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
            _cb.height = CrouchHeight;
            transPlayer.transform.localScale = CrouchScale;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _cb.height = StandHeight;
            transPlayer.transform.localScale = StandScale;
        }
    }

    private void FixedUpdate()
    {
            _rb.velocity = transform.right * Input.GetAxis("Horizontal") * Speed + 
            transform.up * _rb.velocity.y + 
            transform.forward * Input.GetAxis("Vertical") * Speed;
    }
}
