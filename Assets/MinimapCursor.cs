using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.Basic;
using UnityEngine;

public class MinimapCursor : NetworkBehaviour
{
    public GameObject Cursor;
    

    // Start is called before the first frame update
    public override void OnStartLocalPlayer()
    {
        if (isServer && isLocalPlayer)
        {
            Cursor = GameObject.Find("CursorPlayer1");
        }
        else if (isLocalPlayer)
        {
            Cursor =  GameObject.Find("CursorPlayer2");
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
        Cursor.transform.position = new Vector3(gameObject.transform.position.x,8,gameObject.transform.position.z);
    }
}
