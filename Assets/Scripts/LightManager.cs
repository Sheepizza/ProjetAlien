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

    bool _canUse = true;

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
        if (ButtonsLights != null && ButtonsLights.ContainsKey(key) && _canUse)
        {
            _light = GameObject.Find(ButtonsLights[key]);
            bool _isButtonActive = GameObject.Find(key).GetComponent<IsActivate>().IsActive;
            if (_light != null)
            {
                CmdChangeLightPos(_light, _isButtonActive);
                GameObject.Find(key).GetComponent<IsActivate>().IsActive = !_isButtonActive;
                _canUse = false;
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
        Debug.Log(_isActive);
        if (_light.GetComponent<Light>() != null && _isActive)
        {
            _light.GetComponent<Light>().enabled = !_isActive;
            _canUse = true;
        }

        else if (_light.GetComponent<Light>() != null)
        {
            StartCoroutine(LightGlitched(_light));
        }
    }

    IEnumerator LightGlitched(GameObject _light)
    {
        _light.GetComponent<Light>().enabled = true;
        yield return new WaitForSeconds(0.025f);
        _light.GetComponent<Light>().enabled = false;
        yield return new WaitForSeconds(0.05f);
        _light.GetComponent<Light>().enabled = true;
        yield return new WaitForSeconds(0.025f);
        _light.GetComponent<Light>().enabled = false;
        yield return new WaitForSeconds(0.6f);
        _light.GetComponent<Light>().enabled = true;
        yield return new WaitForSeconds(0.05f);
        _light.GetComponent<Light>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        _light.GetComponent<Light>().enabled = true;
        _canUse = true;
    }
}
