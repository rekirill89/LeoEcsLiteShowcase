using System.Collections.Generic;
using LeoEcsShowcase;
using UnityEngine;

[CreateAssetMenu(fileName = "BusinessList", menuName = "Scriptable Objects/BusinessList")]
public class BusinessConfigList : ScriptableObject
{
    public List<BusinessConfig> list = new List<BusinessConfig>();
}
