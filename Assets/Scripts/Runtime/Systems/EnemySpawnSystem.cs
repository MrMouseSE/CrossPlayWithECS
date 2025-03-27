using Runtime.Components;
using Runtime.StaticUtils;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace Runtime.Systems
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
        private Stash<EnemyUnitCount> _enemyUnitCountStash;
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
            _enemyUnitCountStash = World.GetStash<EnemyUnitCount>();
            _healthStash = World.GetStash<HealthComponent>();
            _spawnAreaStash = World.GetStash<SpawnAreaComponent>();
            _attackStash = World.GetStash<AttackComponent>();
            _navMeshAgentStash = World.GetStash<NavMeshAgentComponent>();
            
            _enemyFilter = World.Filter.With<EnemyMarker>().With<UnitComponent>().Build();
            _spawnAreaFilter = World.Filter.With<SpawnAreaComponent>().With<EnemyMarker>().Build();

            var jobHandle = Addressables.LoadAssetAsync<GameObject>("Unit");
            jobHandle.Completed += OnPrefabLoaded;
            
            var jobHandleParams = Addressables.LoadAssetAsync<UnitParameters>("UnitParameters");
            jobHandleParams.Completed += OnUnitParametersLoaded;

            SpawnUtils.CacheSpawnAreas(_spawnAreaFilter, ref _spawnAreaEntities, ref _spawnAreaCount);
            _enemyUnitCountEntity =  World.Filter.With<EnemyUnitCount>().Build().First();
        }

        private void OnPrefabLoaded(AsyncOperationHandle<GameObject> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                _unitPrefab = obj.Result;
            }
        }

        private void OnUnitParametersLoaded(AsyncOperationHandle<UnitParameters> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                _unitParameters = obj.Result;
            }
        }

        public void OnUpdate(float deltaTime)
        {
            if (_unitPrefab == null || _unitParameters == null || _spawnAreaCount == 0)
                return;

            if (SpawnUtils.ShouldSpawnUnit(_enemyFilter,_enemyUnitCountStash.Get(_enemyUnitCountEntity).Count))
            {
                var stashes = new SpawnStashes<EnemyMarker> {
                    MarkerStash = _enemyMarkerStash,
                    HealthStash = _healthStash,
                    AttackStash = _attackStash,
                    NavMeshAgentStash = _navMeshAgentStash,
                    SpawnAreaStash = _spawnAreaStash
                };
                SpawnUtils.SpawnUnit(_unitPrefab, _unitParameters ,stashes, _spawnAreaEntities, _spawnAreaCount);
            }
        }

        public void Dispose() { }
    }
}