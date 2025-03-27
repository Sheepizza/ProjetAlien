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

public class PlayerController : NetworkBehaviour
{
    public Rigidbody _rb;
    public CapsuleCollider _cb;


    [Header ("Speed")]
    public float moveSpeed;
    public float walkSpeed;
    public float crouchSpeed;
    public float sprintSpeed;

    [Space(25)]

    [Header ("Sprint")]
    public float runningTime;
    public float maxRunningTime;
    public float minRunningTime;
    public bool EnSprint;

    [Space(25)]

    [Header ("Crouch")]
    public float t;
    public float crouchYScale;
    private float startYScale;
    public CapsuleCollider _cbCrouch;
    public GameObject camCrouch;
    public GameObject camHold;
    Vector3 camStandPosition;
    float colliderStandHeight;

    [Space(25)]

    [Header ("KeyCode")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;



    void Start()
    {
        moveSpeed = walkSpeed;
        maxRunningTime = runningTime;
        camStandPosition = camHold.transform.localPosition;
        colliderStandHeight = _cb.height;
        startYScale = transform.localScale.y;
    }
    private void FixedUpdate() //Déplacements
    {
        _rb.velocity = transform.right * Input.GetAxis("Horizontal") * moveSpeed +
        transform.up * _rb.velocity.y +
        transform.forward * Input.GetAxis("Vertical") * moveSpeed;
    }


    void Update()
    {
        //LE CROUCH
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            _rb.AddForce(Vector3.down * 2f, ForceMode.Impulse);
            camHold.transform.localPosition = Vector3.Lerp(camHold.transform.localPosition, camCrouch.transform.localPosition, t);
            _cb.height = _cbCrouch.height;
            moveSpeed = crouchSpeed;
            //ajout anim accroupis
        }
        else if (Input.GetKeyUp(crouchKey)) //Se relever
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.up, out hit, 2f)) //Ne peut pas se relever car plafond trop bas
            {
                camHold.transform.localPosition = camCrouch.transform.localPosition;
                _cb.height = _cbCrouch.height;
                moveSpeed = crouchSpeed;
            }
            else //Peut se relever
            {
                transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
                camHold.transform.localPosition = Vector3.Lerp(camHold.transform.localPosition, camStandPosition, t);
                _cb.height = colliderStandHeight;
                moveSpeed = walkSpeed;
                //ajout anim debout
            }
        }

        //SPRINT
        if (Input.GetKeyDown(sprintKey) && moveSpeed == walkSpeed)
        {
            StartCoroutine(DeloadSprint());
        }
        if (Input.GetKeyUp(sprintKey))
        {
            StopCoroutine(DeloadSprint());
            moveSpeed = walkSpeed;
            EnSprint = false;
            StartCoroutine(ReloadSprint());
        }
    }

    



    //Reload et Deload du - SPRINT
    IEnumerator DeloadSprint() 
    {
        EnSprint = true;
        {
            while(EnSprint)
            {
                moveSpeed = sprintSpeed;
                runningTime -= Time.deltaTime;
                if (runningTime <= minRunningTime)
                {
                    moveSpeed = walkSpeed;
                    EnSprint = false;
                }
                Debug.Log("EnSprint");
                yield return null;
            }
        }
    }
    IEnumerator ReloadSprint()
    {
        while(moveSpeed == walkSpeed && runningTime < maxRunningTime)
        {
            runningTime += Time.deltaTime;
            yield return null;
        }
    }
}