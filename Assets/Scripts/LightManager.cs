using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightManager : NetworkBehaviour
{
    public DictionariesDatas lightsDatas;
    Dictionary<string, string> ButtonsLights;

    GameObject _light;

    public override void OnStartLocalPlayer()
    {
        if (isServer && isLocalPlayer)
        {
            ButtonsLights = lightsDatas.GetJ1Dictionary();
        }
        else if (isLocalPlayer)
        {
            ButtonsLights = lightsDatas.GetJ2Dictionary();
        }
    }

    public void ChangeLightState(string key)
    {
        if (ButtonsLights != null && ButtonsLights.ContainsKey(key))
        {
            _light = GameObject.Find(ButtonsLights[key]);
            bool _isButtonActive = GameObject.Find(key).GetComponent<IsActivate>().IsActive;
            if (_light != null)
            {
                CmdChangeLightPos(_light, _isButtonActive);
                GameObject.Find(key).GetComponent<IsActivate>().IsActive = !_isButtonActive;
            }
        }
    }

    [Command]
    void CmdChangeLightPos(GameObject _light, bool _isActive)
    {
        RpcChangeLightPos(_light, _isActive);
    }

    [ClientRpc]
    void RpcChangeLightPos(GameObject _light, bool _isActive)
    {
        if (_light.GetComponent<Light>() != null)
        _light.GetComponent<Light>().enabled = !_isActive;
    }
}
