using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LightManager : NetworkBehaviour
{
    #region CamVariables
    public CamDatas camDatas;
    [SerializeField]
    public List<string> keyCamLists;
    public List<GameObject> camScreens;

    public LightToCamLink lightToCamLink;
    #endregion

    #region LightVariables
    public DictionariesDatas lightsDatas;

    Dictionary<string, string> ButtonsLights;

    GameObject _lightParent;

    bool _canUse = true;
    #endregion

    public override void OnStartLocalPlayer()
    {
        if (isServer && isLocalPlayer)
        {
            ButtonsLights = lightsDatas.GetJ1Dictionary();
            GameObject ScreenListObject = GameObject.Find("CamScreensJ1");
            Debug.Log("Voici " + ScreenListObject);
            for (int i = 0; i < camDatas.J1BC.Count; i++)
            {
                keyCamLists.Add(camDatas.J1BC[i].key);
                if (i <= 9)
                    camScreens.Add(ScreenListObject.transform.GetChild(i).gameObject);
                //Debug.Log(camScreens[i]);
            }
            CmdSetUpScreens();
        }
        else if (isLocalPlayer)
        {
            ButtonsLights = lightsDatas.GetJ2Dictionary();
            GameObject ScreenListObject = GameObject.Find("CamScreensJ2");
            for (int i = 0; i < camDatas.J2BC.Count; i++)
            {
                keyCamLists.Add(camDatas.J2BC[i].key);
                if (i <= 4)
                    camScreens.Add(ScreenListObject.transform.GetChild(i).gameObject);
            }
            CmdSetUpScreens();
        }
    }
    public void ChangeLightState(string key)
    {
        if (ButtonsLights != null && ButtonsLights.ContainsKey(key) && _canUse)
        {
            _lightParent = GameObject.Find(ButtonsLights[key]);
            bool _isButtonActive = GameObject.Find(key).GetComponent<IsActivate>().IsActive;
            if (_lightParent != null)
            {
                if (!_isButtonActive && ElectricityManager.Instance.CompareActivePower() || _isButtonActive)
                {
                    Debug.Log(_lightParent.transform.GetChild(0).gameObject);
                    GameObject.Find(key).GetComponent<IsActivate>().IsActive = !_isButtonActive;
                    CmdChangeLightPos(_lightParent.transform.GetChild(0).gameObject, _isButtonActive);
                    ScreenSwitch(key, _isButtonActive);
                    _canUse = false;
                }
            }
        }
    }

    [Command]
    void CmdChangeLightPos(GameObject _lights, bool _isActive)
    {
        RpcChangeLightPos(_lights, _isActive);
    }

    [ClientRpc]
    void RpcChangeLightPos(GameObject _lights, bool _isActive)
    {
        if (_lights != null && _isActive)
        {
            ElectricityManager.Instance.DecreaseActivePower();
            _lights.SetActive(!_isActive);
            _canUse = true;
        }

        else if (_lights != null)
        {
            ElectricityManager.Instance.IncreaseActivePower();
            StartCoroutine(LightGlitched(_lights));
        }
    }

    IEnumerator LightGlitched(GameObject _lights)
    {
        _lights.SetActive(true);
        yield return new WaitForSeconds(0.025f);
        _lights.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        _lights.SetActive(true);
        yield return new WaitForSeconds(0.025f);
        _lights.SetActive(false);
        yield return new WaitForSeconds(0.6f);
        _lights.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        _lights.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        _lights.SetActive(true);
        _canUse = true;
    }

    [Command]
    public void CMDUnactiveAllLights()
    {
        RPCUnactiveAllLights();
    }

    [ClientRpc]
    void RPCUnactiveAllLights()
    {
        Dictionary<string, string> _dico;
        _dico = lightsDatas.GetJ1Dictionary();

        foreach (var _light in _dico.Values)
        {
            GameObject.Find(_light).transform.GetChild(0).gameObject.SetActive(false);
        }

        _dico = lightsDatas.GetJ2Dictionary();

        foreach (var _light in _dico.Values)
        {
            GameObject.Find(_light).transform.GetChild(0).gameObject.SetActive(false);
        }
    }


    [Command]
    public void CmdSetUpScreens()
    {
        RpcSetUpScreens();
    }

    [ClientRpc]
    public void RpcSetUpScreens()
    {
        for (int i = 0; i < camScreens.Count; i++)
        {
            camScreens[i].GetComponent<MeshRenderer>().material = camDatas.CamMaterialJ1[camDatas.CamMaterialJ1.Count-1];
        }
    }

    public void ScreenSwitch(string key, bool _isActive)
    {
        Debug.Log("Entering ScreenSwitch");
        if (_isActive)
        {
            ScreenOff(key);
        }
        else if (!_isActive)
        {
            ScreenOn(key);
        }
        //Ajouter la condition
    }

    public void ScreenOn(string key)
    {
        Debug.Log("Entering ScreenOn");
        for (int i = 0; i < lightToCamLink.J1LinkList.Count; i++)
        {
            if (key == lightToCamLink.J1LinkList[i].lightKey)
            {
                Debug.Log("Checking Pass - " + lightToCamLink.J1LinkList[i].lightKey);
                for (int j = 0; j < lightToCamLink.J1LinkList[i].CamNumber.Count; j++)
                {
                    camScreens[lightToCamLink.J1LinkList[i].ScreensNumbers[j]].GetComponent<MeshRenderer>().material = camDatas.CamMaterialJ1[lightToCamLink.J1LinkList[i].CamNumber[j]];
                    Debug.Log("Material applied - " + camDatas.CamMaterialJ1[lightToCamLink.J1LinkList[i].CamNumber[j]] + " on " + camScreens[lightToCamLink.J1LinkList[i].ScreensNumbers[j]]);
                }
            }
            //Si le numéro de light correspond au numéro de texure
            //Appliquer la texture d'écran actif
        }
    }
    
    public void ScreenOff(string key)
    {
        Debug.Log("Entering ScreenOff");
        for (int i = 0; i < lightToCamLink.J1LinkList.Count; i++)
        {
            if (key == lightToCamLink.J1LinkList[i].lightKey)
            {
                for (int j = 0; j < lightToCamLink.J1LinkList[i].CamNumber.Count; j++)
                {
                    camScreens[lightToCamLink.J1LinkList[i].ScreensNumbers[j]].GetComponent<MeshRenderer>().material = camDatas.CamMaterialJ1[camDatas.CamMaterialJ1.Count-1];
                }
            }
            //Si le numéro de light correspond au numéro de texure
            //Appliquer la texture noire
        }
    }
}
