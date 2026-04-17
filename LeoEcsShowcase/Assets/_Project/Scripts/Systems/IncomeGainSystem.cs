using Leopotam.EcsLite;
using UnityEngine;

namespace LeoEcsShowcase
{
    public class IncomeGainSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        
        private EcsPool<IncomeComponent> _incomePool;
        private EcsPool<IncomeTimerComponent> _timerPool;
        private EcsPool<UpgradeComponent> _upgradesPool;
        private EcsPool<BalanceComponent> _balancePool;
        private EcsPool<UpdateUIComponent> _updateUIPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            _filter = _world.Filter<IncomeComponent>()
                .Inc<IncomeTimerComponent>()
                .End();
            
            _incomePool = _world.GetPool<IncomeComponent>(); 
            _timerPool = _world.GetPool<IncomeTimerComponent>();
            _balancePool = _world.GetPool<BalanceComponent>();
            _upgradesPool = _world.GetPool<UpgradeComponent>();
            _updateUIPool = _world.GetPool<UpdateUIComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                if (_timerPool.Get(entity).IsReady)
                {
                    ref var balance = ref _balancePool.Get(Game.BalanceEntity);
                    balance.Value += 
                        (_incomePool.Get(entity).Value * _upgradesPool.Get(entity).IncomeMultiplier);
                    
                    ref var component = ref _timerPool.Get(entity);
                    component.IsReady = false;
                    
                    ref var updateUI = ref _updateUIPool.Get(entity);
                    if(!updateUI.IsReady)
                        updateUI.IsReady = true;
                }
            }
        }

    }
}