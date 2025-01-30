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

    // Start is called before the first frame update
    public override void OnStartClient()
    {
        if(LocalConnectionToServer.LocalConnectionId == GameManager.Instance.J2Identity)
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
    }
}
