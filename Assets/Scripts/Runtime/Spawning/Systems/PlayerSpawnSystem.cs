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
    public sealed class PlayerSpawnSystem : ISystem
    {
        public World World { get; set; }
        
        private Filter _playerFilter;
        private Filter _spawnAreaFilter;
        
        private Stash<AttackComponent> _attackStash;
        private Stash<NavMeshAgentComponent> _navMeshAgentStash;
        private Stash<PlayerUnitCount> _playerUnitCountStash;
        private Stash<PlayerMarker> _playerMarkerStash;
        private Stash<HealthComponent> _healthStash;
        private Stash<SpawnAreaComponent> _spawnAreaStash;
        private Entity _playerUnitCountEntity;

        private Entity[] _spawnAreaEntities;
        private int _spawnAreaCount;
        private GameObject _unitPrefab;  
        private UnitParameters _unitParameters;
        
        
        public void OnAwake()
        {
            _playerMarkerStash = World.GetStash<PlayerMarker>();
            _healthStash = World.GetStash<HealthComponent>();
            _playerUnitCountStash = World.GetStash<PlayerUnitCount>();
            _spawnAreaStash = World.GetStash<SpawnAreaComponent>();
            _attackStash = World.GetStash<AttackComponent>();
            _navMeshAgentStash = World.GetStash<NavMeshAgentComponent>();

            _playerFilter = World.Filter.With<PlayerMarker>().With<UnitComponent>().Build();
            _spawnAreaFilter = World.Filter.With<SpawnAreaComponent>().With<PlayerMarker>().Build();
            
            var jobHandle = Addressables.LoadAssetAsync<GameObject>("Unit");
            jobHandle.Completed += OnPrefabLoaded;
            
            var jobHandleParams = Addressables.LoadAssetAsync<UnitParameters>("UnitParameters");
            jobHandleParams.Completed += OnUnitParametersLoaded;
            

            SpawnUtils.CacheSpawnAreas(_spawnAreaFilter, ref _spawnAreaEntities, ref _spawnAreaCount);
            _playerUnitCountEntity =  World.Filter.With<PlayerUnitCount>().Build().First();
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

            if (SpawnUtils.ShouldSpawnUnit(_playerFilter,_playerUnitCountStash.Get(_playerUnitCountEntity).Count))
            {
                var stashes = new SpawnStashes<PlayerMarker> {
                    MarkerStash = _playerMarkerStash,
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
