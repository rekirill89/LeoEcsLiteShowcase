using System;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.UI;

namespace LeoEcsShowcase
{
    public class LogInfoButton : MonoBehaviour
    {
        [SerializeField] private BusinessPanel BusinessEntity;
        private Button _button;
        private EcsWorld _world;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public void Init(EcsWorld world)
        {
            _world = world;
            _button.onClick.AddListener(Call);
        }

        private void Call()
        {
            ref var multiplier = ref _world.GetPool<UpgradeComponent>().Get(BusinessEntity.Entity);
            ref var balance = ref _world.GetPool<BalanceComponent>().Get(Game.BalanceEntity);
            Debug.Log("Multiplier: "+ multiplier.IncomeMultiplier + ", balance:" + balance.Value);
        }
    }
}