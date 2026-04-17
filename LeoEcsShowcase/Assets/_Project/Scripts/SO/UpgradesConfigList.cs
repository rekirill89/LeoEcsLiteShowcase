using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LeoEcsShowcase
{
    [CreateAssetMenu(fileName = "UpgradesList", menuName = "Scriptable Objects/UpgradesList")]
    public class UpgradesConfigList : ScriptableObject
    {
        public List<UpgradeConfig> List = new List<UpgradeConfig>();   
    }

    /*[Serializable]
    public class UpgradePair
    {
        public UpgradeConfig Upgrade1;
        public UpgradeConfig Upgrade2;
    }*/
}
