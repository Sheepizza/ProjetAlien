using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorManager : NetworkBehaviour
{
    public DictionariesDatas doorsDatas;
    Dictionary<string, string> ButtonsDoors;

    GameObject _door;

    public override void OnStartLocalPlayer()
    {
        if (isServer && isLocalPlayer)
        {
            ButtonsDoors = doorsDatas.GetJ1Dictionary();
        }
        else if (isLocalPlayer)
        {
            ButtonsDoors = doorsDatas.GetJ2Dictionary();
        }
    }

    public void ChangeDoorState(string key)
    {

        if (ButtonsDoors != null && ButtonsDoors.ContainsKey(key))
        {
            _door = GameObject.Find(ButtonsDoors[key]);
            bool _isButtonActive = GameObject.Find(key).GetComponent<IsActivate>().IsActive;
            if (_door != null)
            {
                CmdChangeDoorPos(_door,_isButtonActive);
                GameObject.Find(key).GetComponent<IsActivate>().IsActive = !_isButtonActive;
            }
        }
    }

    [Command]
    void CmdChangeDoorPos(GameObject _door, bool _isActive)
    {
        //_door.transform.position = _door.transform.position + Vector3.up * 3 * (_isActive ? -1 : 1);
        RpcChangeDoorPos(_door, _isActive);
    }

    [ClientRpc]
    void RpcChangeDoorPos(GameObject _door, bool _isActive)
    {
        //_door.transform.position = _door.transform.position + Vector3.up * 3 * (_isActive ? -1 : 1);
        StartCoroutine(ChangeDoorPos(_door, _isActive));
    }

    IEnumerator ChangeDoorPos(GameObject _door, bool _isActive)
    {
        Vector3 _startPos = _door.transform.position;
        float _elapsedTime = 0f;
        int _direction = _isActive ? -1 : 1;

        while (_elapsedTime < 1f)
        {
            _door.transform.position = Vector3.Lerp(_startPos, _startPos + Vector3.up * 3 * _direction, _elapsedTime / 1);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        _door.transform.position = _startPos + Vector3.up * 3 * _direction;
    }
}
