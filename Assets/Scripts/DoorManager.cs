using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DoorManager : NetworkBehaviour
{
    public DictionariesDatas doorsDatas;

    public PlayableDirector J1PlayableDirector;
    public PlayableDirector J2PlayableDirector;

    Dictionary<string, string> ButtonsDoors;

    GameObject _door;

    bool _canUse = true;

    public override void OnStartLocalPlayer()
    {
        if (isServer && isLocalPlayer)
        {
            ButtonsDoors = doorsDatas.GetJ1Dictionary();
            //J1PlayableDirector = GameObject.Find("SAS1").GetComponent<PlayableDirector>();
        }
        else if (isLocalPlayer)
        {
            ButtonsDoors = doorsDatas.GetJ2Dictionary();
            //J2PlayableDirector = GameObject.Find("SAS2").GetComponent<PlayableDirector>();
        }
    }

    public void ChangeDoorState(string key)
    {

        if (ButtonsDoors != null && ButtonsDoors.ContainsKey(key) && _canUse)
        {
            Debug.Log("Ici Connard");
            _door = GameObject.Find(ButtonsDoors[key]);
            bool _isButtonActive = GameObject.Find(key).GetComponent<IsActivate>().IsActive;
            Debug.Log(_door);
            Debug.Log(_door.GetComponent<BreakManager>().IsBreak);
            if (_door != null && !_door.GetComponent<BreakManager>().IsBreak)
            {
                Debug.Log("Ici connard 2");
                CmdChangeDoorPos(_door,_isButtonActive);
                GameObject.Find(key).GetComponent<IsActivate>().IsActive = !_isButtonActive;
                _canUse = false;
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
        if(_door.tag == "SAS")
        {

            ChangeSASPos(_door, _isActive);
        }
        else
        {
            StartCoroutine(ChangeDoorPos(_door, _isActive));
        }
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
        _canUse = true;
    }

    void ChangeSASPos(GameObject _door, bool _isActive)
    {
        /*Vector3 _startPos = _door.transform.position;
        float _elapsedTime = 0f;
        int _direction = _isActive ? -1 : 1;

        while (_elapsedTime < 30f)
        {
            _door.transform.position = Vector3.Lerp(_startPos, _startPos + Vector3.up * 3 * _direction, _elapsedTime / 30);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        _door.transform.position = _startPos + Vector3.up * 3 * _direction;*/

        _door.GetComponent<PlayableDirector>().Play();
        
    }
}

