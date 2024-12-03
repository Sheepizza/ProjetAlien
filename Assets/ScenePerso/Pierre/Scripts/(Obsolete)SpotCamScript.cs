using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpotCamScript : MonoBehaviour
{
    [SerializeField] private ScreenScript _screen;
    [SerializeField] private Camera _securityCamera;
    [SerializeField] private PlayerController _playerController;

    private int _currentCamValue = 0;

    private int _lastCamValue;

    private Collider tmpCollider;

    private bool isInCam = false;

    void Start()
    {
        _lastCamValue = _currentCamValue;
        _playerController = gameObject.GetComponentInChildren<PlayerController>();
    }

    //Vérifie si la camera a une valeur et si la touche f est appuyé
    //Si oui, met la caméra a son opposé (Si c'est faux on rentre, si c'est vrai on sort)
    void Update()
    {
        if (_securityCamera && Input.GetKeyDown("f"))
        {
            ToggleCamera();
            if(_securityCamera.enabled == true)
            {
                isInCam = true;
                _playerController.enabled = false;
            }
            else
            {
                isInCam = false;
                _playerController.enabled = true;
            }
        }

        if(_securityCamera && Input.GetKeyDown("q") && isInCam)
        {
            if(_currentCamValue != 0)
            {
                _currentCamValue -= 1;
                ToggleCamera();  
            }
            Debug.Log(_currentCamValue); 
        }

        if(_securityCamera && Input.GetKeyDown("e") && isInCam)
        {
            if(_currentCamValue != _screen._allCameras.Length-1)
            {
                _currentCamValue++;
                ToggleCamera();
            }
            Debug.Log(_currentCamValue);
        }
        
        //vérifie si la cam a changé
        if(_lastCamValue != _currentCamValue && isInCam)
        {
            if(tmpCollider.gameObject.TryGetComponent<ScreenScript>(out ScreenScript cam))
            {
                _securityCamera = cam.GetCamera(_currentCamValue);
                ToggleCamera();
                _lastCamValue = _currentCamValue;
            }
        }
    }


    //Vérifie l'entrée dans le Collider du joueur, si c'est un écran, on appel le constructeur qui assigne une caméra
    private void OnTriggerEnter(Collider col)
    {
        //print("Hello");
        if(col.gameObject.tag == "Screen")
        {
            tmpCollider = col;
            if(col.gameObject.TryGetComponent<ScreenScript>(out ScreenScript cam))//Essaie de prendre le component Caméra venant de ScreenScript
            {
                _screen = cam;
                _lastCamValue = _currentCamValue;
                _securityCamera = cam.GetCamera(_currentCamValue);//Si il le trouve, il lui donne la caméra
            }
        }
    }

    //Quand le joueur sort de la zone, on désactive la caméra et on la désasigne.
    private void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag == "Screen")
        {
            _securityCamera.enabled = false;
            _securityCamera = null;
            tmpCollider = null;
            _screen = null;
        }
    }

    //Active/Désactive la caméra
    private void ToggleCamera()
    {
        _securityCamera.enabled = !_securityCamera.enabled;
    }
}
