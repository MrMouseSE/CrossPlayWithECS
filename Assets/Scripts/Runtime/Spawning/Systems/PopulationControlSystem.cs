using Runtime.Spawning.Components;
using Runtime.Unit.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Runtime.Spawning.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PopulationControlSystem : ISystem
    {
        public World World { get; set; }


        private Stash<PlayerMarker> _playerMarkerStash;
        private Stash<EnemyMarker> _enemyMarkerStash;
        private Stash<UnitCount> _unitCountStash;
        private Entity _playerUnitCountEntity;
        private Entity _enemyUnitCountEntity;

        public void OnAwake()
        {
            _unitCountStash = World.GetStash<UnitCount>();
            _playerMarkerStash = World.GetStash<PlayerMarker>();
            _enemyMarkerStash = World.GetStash<EnemyMarker>();
            
            _enemyUnitCountEntity = World.CreateEntity();
            _enemyMarkerStash.Set(_enemyUnitCountEntity);
            _unitCountStash.Set(_enemyUnitCountEntity, new UnitCount { Value = 5 });
            
            _playerUnitCountEntity = World.CreateEntity();
            _playerMarkerStash.Set(_playerUnitCountEntity);
            _unitCountStash.Set(_playerUnitCountEntity, new UnitCount { Value = 10 });

        }

        public void OnUpdate(float deltaTime)
        {
            
        }

        public void Dispose() { }
    }
}