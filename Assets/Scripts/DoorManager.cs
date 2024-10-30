using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : NetworkBehaviour
{
    public DoorsDatas doorsDatas;
    Dictionary<string, string> ButtonsDoors;

    uint _id;

    public override void OnStartLocalPlayer()
    {
        _id = transform.parent.GetComponent<NetworkIdentity>().netId;

        // Initialisation du dictionnaire une seule fois par joueur
        if (isServer && isLocalPlayer)
        {
            ButtonsDoors = doorsDatas.GetJ1Dictionary();
            Debug.Log("Dictionnaire du Joueur 1 initialisé.");
        }
        else if (isLocalPlayer)
        {
            ButtonsDoors = doorsDatas.GetJ2Dictionary();
            Debug.Log("Dictionnaire du Joueur 2 initialisé.");
        }
    }

    public void ChangeDoorState(string key)
    {

        if (ButtonsDoors != null && ButtonsDoors.ContainsKey(key))
        {
            Debug.Log(ButtonsDoors[key]);

            // Basculer l'état de la porte
            GameObject door = GameObject.Find(ButtonsDoors[key]);
            if (door != null)
            {
                MeshRenderer renderer = door.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.enabled = !renderer.enabled;
                }
                else
                {
                    Debug.LogWarning("Le MeshRenderer est introuvable pour l'objet : " + ButtonsDoors[key]);
                }
            }
            else
            {
                Debug.LogWarning("La porte pour la clé spécifiée est introuvable : " + ButtonsDoors[key]);
            }
        }
        else
        {
            Debug.LogWarning("Clé introuvable dans le dictionnaire ou dictionnaire non initialisé.");
        }
    }
}
