using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        TurnOffAllLights();
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
                    Debug.Log(_lightParent.transform.GetChild(0).gameObject);
                    GameObject.Find(key).GetComponent<IsActivate>().IsActive = !_isButtonActive;
                    CmdChangeLightPos(_lightParent.transform.GetChild(0).gameObject, _isButtonActive);
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

    void TurnOffAllLights()
    {
        foreach (var value in ButtonsLights.Values)
        {
            GameObject.Find(value).transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
