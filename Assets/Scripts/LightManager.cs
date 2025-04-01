using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightManager : NetworkBehaviour
{
    public DictionariesDatas lightsDatas;
    Dictionary<string, string> ButtonsLights;

    GameObject _lightParent;

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
            _lightParent = GameObject.Find(ButtonsLights[key]);
            bool _isButtonActive = GameObject.Find(key).GetComponent<IsActivate>().IsActive;
            if (_lightParent != null)
            {
                if (_isButtonActive && ElectricityManager.Instance.CompareActivePower() || !_isButtonActive)
                {
                    CmdChangeLightPos(_lightParent, _isButtonActive);
                    GameObject.Find(key).GetComponent<IsActivate>().IsActive = !_isButtonActive;
                    _canUse = false;
                }
            }
        }
    }

    [Command]
    void CmdChangeLightPos(GameObject _lightParent, bool _isActive)
    {
        RpcChangeLightPos(_lightParent, _isActive);
    }

    [ClientRpc]
    void RpcChangeLightPos(GameObject _lightParent, bool _isActive)
    {
        Debug.Log(_isActive);
        if (_lightParent != null && _isActive)
        {
            ElectricityManager.Instance.DecreaseActivePower();
            _lightParent.SetActive(!_isActive);
            _canUse = true;
        }

        else if (_lightParent != null)
        {
            ElectricityManager.Instance.IncreaseActivePower();
            StartCoroutine(LightGlitched(_lightParent));
        }
    }

    IEnumerator LightGlitched(GameObject _lightParent)
    {
        _lightParent.SetActive(true);
        yield return new WaitForSeconds(0.025f);
        _lightParent.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        _lightParent.SetActive(true);
        yield return new WaitForSeconds(0.025f);
        _lightParent.SetActive(false);
        yield return new WaitForSeconds(0.6f);
        _lightParent.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        _lightParent.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        _lightParent.SetActive(true);
        _canUse = true;
    }
}
