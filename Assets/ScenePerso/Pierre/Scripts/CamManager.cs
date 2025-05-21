using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class CamManager : NetworkBehaviour
{
    //public Material ButtonOnMaterial;
    //public Material ButtonOffMaterial;
    //public CamDatas camDatas;
    //Dictionary<string, string> CamButtons;
    //List<CamKeyValuePair> CamList;

    //List<Material> CamMaterialsList;

    //[SerializeField]
    //private GameObject screen;
    //GameObject _button;
    //uint _id;
    //int CamMaterialsIndex = 0;

    //public override void OnStartLocalPlayer()
    //{
    //    _id = transform.parent.GetComponent<NetworkIdentity>().netId;
        
    //    // Initialisation du dictionnaire une seule fois par joueur
    //    if (isServer && isLocalPlayer)
    //    {
    //        CamButtons = camDatas.GetJ1Dictionary();
    //        Debug.Log("Dictionnaire de cameras du Joueur 1 initialis�.");
    //        /*foreach (KeyValuePair<string,string> keyValuePair in CamButtons)
    //        {
    //            Debug.Log(keyValuePair);
    //        }*/
    //        screen = GameObject.Find("CamScreenJ1");
    //        CamList = camDatas.J1BC;
    //        CamMaterialsList = camDatas.CamMaterialJ1;
    //        Debug.Log(CamMaterialsList);
    //    }
    //    else if (isLocalPlayer)
    //    {
    //        CamButtons = camDatas.GetJ2Dictionary();
    //        Debug.Log("Dictionnaire de cameras du Joueur 2 initialis�.");
    //        screen = GameObject.Find("CamScreenJ2");
    //        CamList = camDatas.J2BC;
    //        CamMaterialsList = camDatas.CamMaterialJ2;
    //    }
    //    screen.GetComponent<MeshRenderer>().material = CamMaterialsList[CamMaterialsIndex];
    //}

    //public void ChangeCam(string key)
    //{
    //    Debug.Log("Bonjour");
    //    if (CamButtons != null && CamButtons.ContainsKey(key))
    //    {
    //        //Debug.Log(CamButtons[key]);

    //        switch (key)
    //        {
    //            case "ButPrevJ1" : 
    //                Debug.Log(key);
    //                _button = GameObject.Find("ButPrevJ1");
    //                Debug.Log("J'ai le button : " + _button.name);
    //                StartCoroutine(SwapButtonColor(_button));
    //                Debug.Log(CamMaterialsIndex);
    //                if (CamMaterialsIndex > 0)
    //                {
    //                    CamMaterialsIndex -=1;
    //                    StartCoroutine(SwapScreenMaterial(screen));
    //                }
    //                else Debug.Log("Pas possbile mon fraté");
    //            break;

    //            case "ButNextJ1" : 
    //                _button = GameObject.Find("ButNextJ1");
    //                StartCoroutine(SwapButtonColor(_button));
    //                Debug.Log(CamMaterialsIndex);
    //                if(CamMaterialsIndex < CamMaterialsList.Count-1)
    //                {
    //                    CamMaterialsIndex++;
    //                    StartCoroutine(SwapScreenMaterial(screen));
    //                }
    //                else Debug.Log("Pas Possible mon fraté");
    //            break;

    //            case "ButPrevJ2" : 
    //                Debug.Log(key);
    //                _button = GameObject.Find("ButPrevJ2");
    //                Debug.Log("J'ai le button : " + _button.name);
    //                StartCoroutine(SwapButtonColor(_button));
    //                Debug.Log(CamMaterialsIndex);
    //                if (CamMaterialsIndex > 0)
    //                {
    //                    CamMaterialsIndex -=1;
    //                    StartCoroutine(SwapScreenMaterial(screen));
    //                }
    //                else Debug.Log("Pas possbile mon fraté");
    //            break;

    //            case "ButNextJ2" : 
    //                _button = GameObject.Find("ButNextJ2");
    //                StartCoroutine(SwapButtonColor(_button));
    //                Debug.Log(CamMaterialsIndex);
    //                if(CamMaterialsIndex < CamMaterialsList.Count-1)
    //                {
    //                    CamMaterialsIndex++;
    //                    StartCoroutine(SwapScreenMaterial(screen));
    //                }
    //                else Debug.Log("Pas Possible mon fraté");
    //            break;

    //            default : Debug.LogWarning("Pas bon la");
    //            break;
    //        }

    //        /*GameObject cam = GameObject.Find(CamButtons[key]);
    //        if (cam != null)
    //        {
    //            Debug.Log("J'ai la Cam");
    //            int CamIndex = CamList.FindIndex(item => item.value == CamButtons[key]);

    //            if (CamIndex != -1)
    //            {
    //                Debug.Log("Index de la caméra trouvée : " + CamIndex);
    //                //screen.GetComponent<MeshRenderer>().material = CamList[CamIndex].CamMaterial;
    //            }
    //        }
    //        else
    //        {
    //            Debug.LogWarning("La cam pour la cle specifiee est introuvable : " + CamButtons[key]);
    //        }*/
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Cl� introuvable dans le dictionnaire ou dictionnaire non initialis�.");
    //    }
    //}

    //public IEnumerator SwapButtonColor(GameObject _button)
    //{
    //    Debug.Log("change couleur");
    //    //Passe la couleur à rouge
    //    _button.GetComponent<MeshRenderer>().material = ButtonOnMaterial;
    //    yield return new WaitForSeconds(1);
    //    //Enlève le rouge
    //    _button.GetComponent<MeshRenderer>().material = ButtonOffMaterial;

    //}

    //public IEnumerator SwapScreenMaterial(GameObject _screen)
    //{
    //    screen.GetComponent<MeshRenderer>().material = CamMaterialsList[0];
    //    yield return new WaitForSeconds(1);
    //    screen.GetComponent<MeshRenderer>().material = CamMaterialsList[CamMaterialsIndex];
    //}

}
