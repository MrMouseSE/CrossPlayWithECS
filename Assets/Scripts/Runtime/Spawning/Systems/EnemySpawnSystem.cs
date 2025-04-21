using Runtime.Combat.Components;
using Runtime.Movement.Components;
using Runtime.Spawning.Components;
using Runtime.Spawning.Utils;
using Runtime.Unit.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.Spawning.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class EnemySpawnSystem : ISystem
    {
        public World World { get; set; }

        private Filter _enemyFilter;
        private Filter _spawnAreaFilter;
        
        private Stash<EnemyMarker> _enemyMarkerStash;
        private Stash<HealthComponent> _healthStash;
        private Stash<SpawnAreaComponent> _spawnAreaStash;
        private Stash<AttackComponent> _attackStash;
        private Stash<NavMeshAgentComponent> _navMeshAgentStash;
        
        private Entity[] _spawnAreaEntities;
        private Entity _enemyUnitCountEntity;
        private int _spawnAreaCount;
        
        private UnitParameters _unitParameters;
        private GameObject _unitPrefab;

        public void OnAwake()
        {
            _enemyMarkerStash = World.GetStash<EnemyMarker>();
            _healthStash = World.GetStash<HealthComponent>();
            _spawnAreaStash = World.GetStash<SpawnAreaComponent>();
            _attackStash = World.GetStash<AttackComponent>();
            _navMeshAgentStash = World.GetStash<NavMeshAgentComponent>();
            
            _enemyFilter = World.Filter.With<EnemyMarker>().With<UnitComponent>().Build();
            _spawnAreaFilter = World.Filter.With<SpawnAreaComponent>().With<EnemyMarker>().Build();
            
            SpawnUtils.CacheSpawnAreas(_spawnAreaFilter, ref _spawnAreaEntities, ref _spawnAreaCount);
        }
      

        public void OnUpdate(float deltaTime)
        {
            if ( _unitParameters == null || _spawnAreaCount == 0)
                return;

            // if (SpawnUtils.ShouldSpawnUnit(_enemyFilter,_enemyUnitCountStash.Get(_enemyUnitCountEntity).Value))
            // {
            //     var stashes = new SpawnStashes<EnemyMarker>
            //     {
            //         MarkerStash = _enemyMarkerStash,
            //         HealthStash = _healthStash,
            //         AttackStash = _attackStash,
            //         NavMeshAgentStash = _navMeshAgentStash,
            //         SpawnAreaStash = _spawnAreaStash
            //     };
            //     
            //     
            //     //SpawnUtils.SpawnUnit(_unitParameters ,stashes, _spawnAreaEntities, _spawnAreaCount);
            // }
        }

        public void Dispose() { }
    }
}