using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CamManager : NetworkBehaviour
{
    public CamDatas camDatas;
    Dictionary<string, string> CamButtons;

    List<CamKeyValuePair> CamList;

    [SerializeField]
    private GameObject screen;

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
            screen = GameObject.Find("CamScreenJ1");
            CamList = camDatas.J1BC;
        }
        else if (isLocalPlayer)
        {
            CamButtons = camDatas.GetJ2Dictionary();
            Debug.Log("Dictionnaire de cameras du Joueur 2 initialis�.");
            screen = GameObject.Find("CamScreenJ2");
            CamList = camDatas.J2BC;
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
                int index = CamList.FindIndex(item => item.value == CamButtons[key]);

                if (index != -1)
                {
                    Debug.Log("Index de la caméra trouvée : " + index);
                    screen.GetComponent<MeshRenderer>().material = CamList[index].CamMaterial;
                }
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
