using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/PickableObjects", order = 1)]
public class PickableObjects : ScriptableObject
{
    public List<string> PickableObjectsTagName;
}
