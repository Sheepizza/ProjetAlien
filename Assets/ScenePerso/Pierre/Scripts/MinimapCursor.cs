using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.BouncyCastle.Tls;
using Mirror.Examples.Basic;
using Telepathy;
using UnityEngine;

public class MinimapCursor : NetworkBehaviour
{
    public GameObject CursorJ1;
    public Transform FollowJ1;
    public GameObject CursorJ2;
    public Transform FollowJ2;
    private float timer = 0;

    // Start is called before the first frame update
    public void SetupMiniMap()
    {
        if (isServer && isLocalPlayer)
        {
            //Debug.Log("GetCursor");
            CursorJ1 = GameObject.Find("CursorPlayer1");
            CursorJ2 = GameObject.Find("CursorPlayer2");
            FollowJ1 = GameManager.Instance.J1.transform;
            FollowJ2 = GameManager.Instance.J2.transform;
        }
    }

    void Update()
    {
        UpdateCursorPos();

    }
    public void UpdateCursorPos()
    {
        
        if (isServer && isLocalPlayer && CursorJ1 != null && CursorJ2 != null)
        {
            CursorJ1.transform.position = new Vector3(FollowJ1.transform.position.x,6f,FollowJ1.transform.position.z);
            CursorJ2.transform.position = new Vector3(FollowJ2.transform.position.x,6f,FollowJ2.transform.position.z);
        }
        timer += Time.deltaTime;
        if (timer>0.2f && CursorJ1 != null && CursorJ2 != null)
        {
            //UpdateClientCursorPos();
            timer = 0;
        }
    }
}
