using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenScript : MonoBehaviour
{
    public Camera[] _allCameras;

    //Constructeur permettant d'assigner une camera
    public Camera GetCamera(int _currentCam)
    {
        return _allCameras[_currentCam]; //Donne la caméra
    }
    


}
