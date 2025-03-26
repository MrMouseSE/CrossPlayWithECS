using Runtime.Components;
using Runtime.StaticUtils;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PlayerSpawnSystem : ISystem
    {
        public World World { get; set; }
        
        private Filter _playerFilter;
        private Filter _spawnAreaFilter;
        
        private GameObject _unitPrefab;
        private Stash<PlayerUnitCount> _playerUnitCountStash;
        private Stash<PlayerMarker> _playerMarkerStash;
        private Stash<HealthComponent> _healthStash;
        private Stash<SpawnAreaComponent> _spawnAreaStash;
        private Entity _playerUnitCountEntity;

        private Entity[] _spawnAreaEntities;
        private int _spawnAreaCount;

        public void OnAwake()
        {
            _playerMarkerStash = World.GetStash<PlayerMarker>();
            _healthStash = World.GetStash<HealthComponent>();
            _playerUnitCountStash = World.GetStash<PlayerUnitCount>();
            _spawnAreaStash = World.GetStash<SpawnAreaComponent>();

            _playerUnitCountEntity = World.CreateEntity();
            _playerUnitCountStash.Set(_playerUnitCountEntity, new PlayerUnitCount { Count = 5 });

            _playerFilter = World.Filter.With<PlayerMarker>().With<UnitComponent>().Build();
            _spawnAreaFilter = World.Filter.With<SpawnAreaComponent>().With<PlayerMarker>().Build();
            
            var jobHandle = Addressables.LoadAssetAsync<GameObject>("Unit");
            jobHandle.Completed += OnPrefabLoaded;

            SpawnUtils.CacheSpawnAreas(_spawnAreaFilter, _spawnAreaStash, ref _spawnAreaEntities, ref _spawnAreaCount);
          
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

            if (SpawnUtils.ShouldSpawnUnit(_playerFilter, _playerUnitCountStash.Get(_playerUnitCountEntity).Count))
            {
                SpawnUtils.SpawnUnit(_unitPrefab, _playerMarkerStash, _healthStash, _spawnAreaEntities, _spawnAreaCount, _spawnAreaStash);
            }
        }

        public void Dispose()
        {
        }
    }
}
