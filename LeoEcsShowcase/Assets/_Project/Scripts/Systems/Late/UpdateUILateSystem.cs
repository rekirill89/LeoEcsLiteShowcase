using Leopotam.EcsLite;

namespace LeoEcsShowcase
{
    public class UpdateUILateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsFilter _blockFilter;
        
        private EcsPool<UpdateUIComponent> _updateUIRequestPool;
        private EcsPool<IncomeComponent> _incomePool;
        private EcsPool<LevelComponent> _levelPool;
        private EcsPool<BalanceUIComponent> _balanceUIPool;
        private EcsPool<BalanceComponent> _balancePool;
        private EcsPool<BlockButtonRequestComponent> _blockPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<UpdateUIComponent>().End();
            _blockFilter = _world.Filter<BlockButtonRequestComponent>().End();
            
            _updateUIRequestPool = _world.GetPool<UpdateUIComponent>();
            _incomePool = _world.GetPool<IncomeComponent>();
            _levelPool = _world.GetPool<LevelComponent>();
            _balanceUIPool = _world.GetPool<BalanceUIComponent>();
            _balancePool = _world.GetPool<BalanceComponent>();
            _blockPool = _world.GetPool<BlockButtonRequestComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            _balanceUIPool.Get(Game.BalanceEntity).Value.text = _balancePool.Get(Game.BalanceEntity).Value.ToString() + "$";
            foreach (var entity in _filter)
            {
                ref var updateComp = ref _updateUIRequestPool.Get(entity);
                if (!updateComp.IsReady)
                    continue;
                
                UpdateUI(entity, updateComp.BusinessPanel);
                
                updateComp.IsReady = false;
            }

            foreach (var entity in _blockFilter)
            {
                _blockPool.Get(entity).UpgradeButton.LockButton();
                
                _blockPool.Del(entity);
            }
        }

        public void UpdateUI(int entity, BusinessPanel businessPanel)
        {
            ref var income = ref _incomePool.Get(entity);
            ref var level = ref _levelPool.Get(entity);
            
            businessPanel.SetIncome(income.Value);
            businessPanel.SetLvl(level.Value);
            businessPanel.SetPrice(level.Price);
        }
    }
}