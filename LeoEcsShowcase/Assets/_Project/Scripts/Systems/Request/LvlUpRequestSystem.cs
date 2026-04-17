using Leopotam.EcsLite;
using UnityEngine;

namespace LeoEcsShowcase
{
    public class LvlUpRequestSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<BalanceComponent> _balancePool;
        private EcsPool<LevelComponent> _levelPool;
        private EcsPool<LvlUpRequestComponent> _requestPool;
        private EcsPool<UpdateUIComponent> _updateUIPool;
        private EcsPool<IncomeComponent> _incomePool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<LvlUpRequestComponent>().End();

            _balancePool = _world.GetPool<BalanceComponent>();
            _levelPool = _world.GetPool<LevelComponent>();
            _incomePool = _world.GetPool<IncomeComponent>();
            _requestPool = _world.GetPool<LvlUpRequestComponent>();
            _updateUIPool = _world.GetPool<UpdateUIComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            ref var balance = ref _balancePool.Get(Game.BalanceEntity);

            foreach (var entity in _filter)
            {
                ref var levelComp = ref _levelPool.Get(entity); 
                ref var incomeComp = ref _incomePool.Get(entity); 
                if (balance.Value >= levelComp.Price)
                {
                    balance.Value -= levelComp.Price;
                    
                    levelComp.Value++;
                    levelComp.Price = (levelComp.Value + 1) * levelComp.InitPrice;
                    incomeComp.Value = incomeComp.InitValue * levelComp.Value;
                    
                    ref var updateUI = ref _updateUIPool.Get(entity);
                    if(!updateUI.IsReady)
                        updateUI.IsReady = true;
                    
                    Debug.Log("LvlUp!");
                }
                else
                    Debug.Log("Not enough money!");
                
                _requestPool.Del(entity);
            }
        }
    }
}