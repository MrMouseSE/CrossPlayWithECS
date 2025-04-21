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

        private Stash<AssignedDefensePoint> _assignedPointStash;
        private Stash<IsOccupied> _isOccupiedStash;

        private Entity _enemyUnitCountEntity;
        private Entity _playerUnitCountEntity;

        public void OnAwake()
        {
            _healthPlayerFilter = World.Filter.With<HealthComponent>().With<PlayerMarker>().Build();
            _healthEnemyFilter = World.Filter.With<HealthComponent>().With<EnemyMarker>().Build();

            _healthStash = World.GetStash<HealthComponent>();
            _unitStash = World.GetStash<UnitComponent>();
            _assignedPointStash = World.GetStash<AssignedDefensePoint>();
            _isOccupiedStash = World.GetStash<IsOccupied>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var healthEntity in _healthPlayerFilter)
            {
                IsDeath(healthEntity, true);
            }
            
            foreach (var healthEntity in _healthEnemyFilter)
            {
                IsDeath(healthEntity, false);
            }
        }
        
        private bool IsDeath(Entity healthEntity, bool isPlayer)
        {
            ref var health = ref _healthStash.Get(healthEntity);
            if (health.HealthPoints <= 0)
            {
                if (isPlayer)
                {
                    ref var assignedPoint = ref _assignedPointStash.Get(healthEntity);
                    if (!World.IsDisposed(assignedPoint.TargetPointEntity) && _isOccupiedStash.Has(assignedPoint.TargetPointEntity))
                    {
                        _isOccupiedStash.Remove(assignedPoint.TargetPointEntity);
                    }
                }
                
                if (_unitStash.Has(healthEntity)) 
                {
                     ref var unit = ref _unitStash.Get(healthEntity);
                     Object.Destroy(unit.RootGameObject);
                }
                World.RemoveEntity(healthEntity);
                return true;
            }
            return false;
        }
        public void Dispose() { }
    }
}