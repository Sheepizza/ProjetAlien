using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.Basic;
using UnityEngine;

public class MinimapCursor : NetworkBehaviour
{
    public GameObject Cursor;

    public GameObject Player;

    // Start is called before the first frame update
    public override void OnStartLocalPlayer()
    {
        if (isServer && isLocalPlayer)
        {
            Cursor = GameObject.Find("CursorPlayer1");
            Player = GameObject.Find("Player1");
        }
        else if (isLocalPlayer)
        {
            Cursor =  GameObject.Find("CursorPlayer2");
            Player = GameObject.Find("Player2");
        }
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        ChangeCursorPos();
    }

    [Command]
    void ChangeCursorPos()
    {
        RpcChangeCursorPos();
    }

    [ClientRpc]
    void RpcChangeCursorPos()
    {
        Cursor.transform.position = new Vector3(Player.transform.position.x,8,Player.transform.position.z);
    }
}
