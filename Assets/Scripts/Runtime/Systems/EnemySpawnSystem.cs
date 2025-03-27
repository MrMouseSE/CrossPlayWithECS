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

        private GameObject _unitPrefab;
        private Stash<EnemyMarker> _enemyMarkerStash;
        private Stash<EnemyUnitCount> _enemyUnitCountStash;
        private Stash<HealthComponent> _healthStash;
        private Stash<SpawnAreaComponent> _spawnAreaStash;
        private Entity[] _spawnAreaEntities;
        private Entity _enemyUnitCountEntity;
        private int _spawnAreaCount;

        public void OnAwake()
        {
            _enemyMarkerStash = World.GetStash<EnemyMarker>();
            _enemyUnitCountStash = World.GetStash<EnemyUnitCount>();
            _healthStash = World.GetStash<HealthComponent>();
            _spawnAreaStash = World.GetStash<SpawnAreaComponent>();
            
            _enemyUnitCountEntity = World.CreateEntity();
            _enemyUnitCountStash.Set(_enemyUnitCountEntity, new EnemyUnitCount() { Count = 10 });

            _enemyFilter = World.Filter.With<EnemyMarker>().With<UnitComponent>().Build();
            _spawnAreaFilter = World.Filter.With<SpawnAreaComponent>().With<EnemyMarker>().Build();

            var jobHandle = Addressables.LoadAssetAsync<GameObject>("Unit");
            jobHandle.Completed += OnPrefabLoaded;

            SpawnUtils.CacheSpawnAreas(_spawnAreaFilter, ref _spawnAreaEntities, ref _spawnAreaCount);
        }

        private void OnPrefabLoaded(AsyncOperationHandle<GameObject> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                _unitPrefab = obj.Result;
            }
        }

        public void OnUpdate(float deltaTime)
        {
            if (_unitPrefab == null || _spawnAreaCount == 0)
                return;

            if (SpawnUtils.ShouldSpawnUnit(_enemyFilter,_enemyUnitCountStash.Get(_enemyUnitCountEntity).Count))
            {
                SpawnUtils.SpawnUnit(_unitPrefab, _enemyMarkerStash, _healthStash, _spawnAreaEntities, _spawnAreaCount,
                    _spawnAreaStash);
            }
        }

        public void Dispose()
        {
        }
    }
}