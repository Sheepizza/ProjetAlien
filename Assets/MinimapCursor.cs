using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.Basic;
using UnityEngine;

public class MinimapCursor : NetworkBehaviour
{
    public GameObject Cursor;
    public Transform Follow;
    

    // Start is called before the first frame update
    public override void OnStartLocalPlayer()
    {
        if (isServer && isLocalPlayer)
        {
            Cursor = GameObject.Find("CursorPlayer1");
            Follow = gameObject.GetComponent<Transform>();
        }
        else if (isLocalPlayer)
        {
            Cursor =  GameObject.Find("CursorPlayer2");
            Follow = gameObject.GetComponent<Transform>();
        }
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        ChangeCursorPos();
    }

    //[Command]
    void ChangeCursorPos()
    {   
        if(Cursor != null)
        {

            Cursor.transform.position = new Vector3(Follow.transform.position.x,8,Follow.transform.position.z);
            Debug.Log("Cursor Pos : " + Cursor.transform.position);
        }
        /*Debug.Log("Commande lancée");
        RpcChangeCursorPos();*/
    }

    /*[ClientRpc]
    void RpcChangeCursorPos()
    {
        Debug.Log("Commande envoyée");
        if (Cursor != null)
            Cursor.transform.position = new Vector3(gameObject.transform.position.x,8,gameObject.transform.position.z);
    }*/
}
