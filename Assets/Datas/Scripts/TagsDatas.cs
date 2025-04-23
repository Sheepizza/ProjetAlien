using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/TagsDatas", order = 1)]
public class TagsDatas : ScriptableObject
{
    public List<string> TagsNames;
}
