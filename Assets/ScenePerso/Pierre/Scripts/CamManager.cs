using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CamManager : NetworkBehaviour
{
    public CamDatas camDatas;

    Dictionary<string, string> CamButtons;

    uint _id;

    public override void OnStartLocalPlayer()
    {
        _id = transform.parent.GetComponent<NetworkIdentity>().netId;
        
        // Initialisation du dictionnaire une seule fois par joueur
        if (isServer && isLocalPlayer)
        {
            CamButtons = camDatas.GetJ1Dictionary();
            Debug.Log("Dictionnaire de cameras du Joueur 1 initialis�.");
            /*foreach (KeyValuePair<string,string> keyValuePair in CamButtons)
            {
                Debug.Log(keyValuePair);
            }*/
        }
        else if (isLocalPlayer)
        {
            CamButtons = camDatas.GetJ2Dictionary();
            Debug.Log("Dictionnaire de cameras du Joueur 2 initialis�.");
        }
    }

    public void ChangeCam(string key)
    {
        Debug.Log("Bonjour");
        if (CamButtons != null && CamButtons.ContainsKey(key))
        {
            Debug.Log(CamButtons[key]);

            // Basculer l'�tat de la porte
            GameObject cam = GameObject.Find(CamButtons[key]);
            if (cam != null)
            {
                Debug.Log("J'ai la Cam");
            }
            else
            {
                Debug.LogWarning("La cam pour la cle specifiee est introuvable : " + CamButtons[key]);
            }
        }
        else
        {
            Debug.LogWarning("Cl� introuvable dans le dictionnaire ou dictionnaire non initialis�.");
        }
    }

}
