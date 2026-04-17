using UnityEngine;

namespace LeoEcsShowcase
{
    [CreateAssetMenu(fileName = "UpgradeConfig", menuName = "Scriptable Objects/UpgradeConfig")]
    public class UpgradeConfig : ScriptableObject
    {
        public string Name;
        public float IncomeMultiplier;
        public float Price;
        public int UpgradeButtonID;
    }
}