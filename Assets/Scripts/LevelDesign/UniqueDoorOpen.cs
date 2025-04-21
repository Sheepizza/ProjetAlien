using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class UniqueDoorOpen : NetworkBehaviour
{
    [Command(requiresAuthority = false)]
    public void OpenDoor(GameObject _door)
    {
        StartCoroutine(DoOpen(_door));
    }
    public IEnumerator DoOpen(GameObject _door)
    {
        Debug.Log("J'ouvre" + _door.name);
        Vector3 _startPos = _door.transform.position;
        float _elapsedTime = 0f;
        int _direction = 1;

        while (_elapsedTime < 1f)
        {
            _door.transform.position = Vector3.Lerp(_startPos, _startPos + Vector3.up * 3 * _direction, _elapsedTime / 1);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        _door.transform.position = _startPos + Vector3.up * 3 * _direction;
    }
}
