using System.Linq;
using LeoEcsShowcase;
using Leopotam.EcsLite;
using UnityEngine;

namespace LeoEcsShowcase
{
    public class UpgradeRequestSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _shared;
        private EcsFilter _filter;
        private EcsPool<BalanceComponent> _balancePool;
        private EcsPool<UpgradeRequestComponent> _upgradePool;
        private EcsPool<UpdateUIComponent> _updateUIPool;
        private EcsPool<BlockButtonRequestComponent> _blockPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _shared = systems.GetShared<SharedData>();
            _filter = _world.Filter<UpgradeRequestComponent>().End();
            _balancePool = _world.GetPool<BalanceComponent>();
            _upgradePool = _world.GetPool<UpgradeRequestComponent>();
            _updateUIPool = _world.GetPool<UpdateUIComponent>();
            _blockPool = _world.GetPool<BlockButtonRequestComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            ref var balance = ref _balancePool.Get(Game.BalanceEntity);

            foreach (var entity in _filter)
            {
                ref var request = ref _world.GetPool<UpgradeRequestComponent>().Get(entity);
                int buttonID = request.UpgradeButton.ID;

                var upgradeConfig = _shared
                    .UpgradesConfigList
                    .List
                    .First(x => x.UpgradeButtonID == buttonID);

                if (balance.Value >= upgradeConfig.Price)
                {
                    ref var upgradesComp = ref _world.GetPool<UpgradeComponent>().Get(entity);
    
                    upgradesComp.IncomeMultiplier += upgradeConfig.IncomeMultiplier;
                    balance.Value -= upgradeConfig.Price;
    
                    _blockPool.Add(entity).UpgradeButton = request.UpgradeButton;
                    Debug.Log("Upgraded!");
                }
                Debug.Log("Not enough money!");
                
                _upgradePool.Del(entity);
            }
        }

    }
}