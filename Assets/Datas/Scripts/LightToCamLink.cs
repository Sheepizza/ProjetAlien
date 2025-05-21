using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class LinkLightCam
{
    public string lightKey;
    public List<int> ScreensNumbers;

    public List<int> CamNumber; 
}

[CreateAssetMenu(fileName = "Data", menuName = "Data/LightToCamLink", order = 1)]
public class LightToCamLink : ScriptableObject
{
    public List<LinkLightCam> J1LinkList;
    public List<LinkLightCam> J2LinkList;
}