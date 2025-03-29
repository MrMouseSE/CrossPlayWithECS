using Runtime.Spawning.Components;
using Runtime.Unit.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Runtime.Combat.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DeathSystem : ISystem
    {
        public World World { get; set; }

        private Filter _healthPlayerFilter;
        private Filter _healthEnemyFilter;
        
        private Stash<HealthComponent> _healthStash;
        private Stash<UnitComponent> _unitStash;
        private Stash<EnemyUnitCount> _enemyUnitCountStash;
        private Stash<PlayerUnitCount> _playerUnitCountStash;
        
        private Entity _enemyUnitCountEntity;
        private Entity _playerUnitCountEntity;

        public void OnAwake()
        {
            _healthPlayerFilter = World.Filter.With<HealthComponent>().With<PlayerMarker>().Build();
            _healthEnemyFilter = World.Filter.With<HealthComponent>().With<EnemyMarker>().Build();
            
            _enemyUnitCountEntity =  World.Filter.With<EnemyUnitCount>().Build().First();
            _playerUnitCountEntity =  World.Filter.With<PlayerUnitCount>().Build().First();
            
            
            _healthStash = World.GetStash<HealthComponent>();
            _unitStash = World.GetStash<UnitComponent>();
            
            _enemyUnitCountStash = World.GetStash<EnemyUnitCount>();
            _playerUnitCountStash = World.GetStash<PlayerUnitCount>();
            
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var healthEntity in _healthPlayerFilter)
            {
                if (IsDeath(healthEntity))
                {
                    ref var countUnit = ref _playerUnitCountStash.Get(_playerUnitCountEntity);
                    countUnit.Count--;
                }
            }
            
            foreach (var healthEntity in _healthEnemyFilter)
            {
                if (IsDeath(healthEntity))
                {
                    ref var countUnit = ref _enemyUnitCountStash.Get(_enemyUnitCountEntity);
                    countUnit.Count--;
                }
            }
        }

        private bool IsDeath(Entity healthEntity)
        {
            ref var health = ref _healthStash.Get(healthEntity);
            if (health.HealthPoints <= 0)
            {
                ref var unit = ref _unitStash.Get(healthEntity);
                Object.Destroy(unit.RootGameObject);
                World.RemoveEntity(healthEntity);
                return true; 
            }

            return false;
        }

        public void Dispose() { }
    }
}