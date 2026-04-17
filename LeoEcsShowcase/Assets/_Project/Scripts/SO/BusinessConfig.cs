using UnityEngine;

namespace LeoEcsShowcase
{
    [CreateAssetMenu(fileName = "BusinessConfig", menuName = "Scriptable Objects/BusinessConfig")]
    public class BusinessConfig : ScriptableObject
    {
        public string Name;
        public float Income;
        public float IncomeTimer;
        public int LvlPrice;
        public int InitLvl;
        public int ID;
    }
}
