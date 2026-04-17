using Leopotam.EcsLite;
using UnityEngine;

namespace LeoEcsShowcase
{
    public class IncomeTimerSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<IncomeTimerComponent> _timerPool;
        private EcsPool<LevelComponent> _levelPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            _filter = _world.Filter<IncomeTimerComponent>().End();
            _timerPool = _world.GetPool<IncomeTimerComponent>();
            _levelPool = _world.GetPool<LevelComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                if (_levelPool.Get(entity).Value == 0)
                    continue;
                
                ref var entityTimer = ref _timerPool.Get(entity);
                entityTimer.CurrentValue += Time.deltaTime;

                if (entityTimer.CurrentValue >= entityTimer.DefaultValue)
                {
                    entityTimer.CurrentValue = 0;
                    entityTimer.IsReady = true;
                }
            }
        }

    }
}