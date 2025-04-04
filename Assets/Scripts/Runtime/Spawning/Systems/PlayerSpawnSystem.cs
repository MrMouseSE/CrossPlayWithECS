using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Runtime.Combat.Components;
using Runtime.Movement.Components;
using Runtime.Spawning.Components;
using Runtime.Spawning.Utils;
using Runtime.Unit.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = UnityEngine.Random;

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
        private Stash<UnitCount> _playerUnitCountStash;
        private Stash<PlayerMarker> _playerMarkerStash;
        private Stash<HealthComponent> _healthStash;
        private Stash<SpawnAreaComponent> _spawnAreaStash;
        private Entity _playerUnitCountEntity;

        private Entity[] _spawnAreaEntities;
        private int _spawnAreaCount;
        private readonly List<UnitParameters> _units = new();
        private SpawnStashes<PlayerMarker> _stashes;


        public void OnAwake()
        {
            _playerMarkerStash = World.GetStash<PlayerMarker>();
            _healthStash = World.GetStash<HealthComponent>();
            _playerUnitCountStash = World.GetStash<UnitCount>();
            _spawnAreaStash = World.GetStash<SpawnAreaComponent>();
            _attackStash = World.GetStash<AttackComponent>();
            _navMeshAgentStash = World.GetStash<NavMeshAgentComponent>();

            _playerFilter = World.Filter.With<PlayerMarker>().With<UnitComponent>().With<NavMeshAgentComponent>().Build();
            _spawnAreaFilter = World.Filter.With<SpawnAreaComponent>().With<PlayerMarker>().Build();
            _playerUnitCountEntity =  World.Filter.With<UnitCount>().Build().First();
            
            SpawnUtils.CacheSpawnAreas(_spawnAreaFilter, ref _spawnAreaEntities, ref _spawnAreaCount);

            for (int i = 0; i < SpawnUtils.KeyUnits.Length; i++)
            {
               OnUnitLoaded(SpawnUtils.KeyUnits[i]);
            }

            _stashes = new SpawnStashes<PlayerMarker>
            {
                MarkerStash = _playerMarkerStash,
                HealthStash = _healthStash,
                AttackStash = _attackStash,
                NavMeshAgentStash = _navMeshAgentStash,
                SpawnAreaStash = _spawnAreaStash
            };
        }

        private async void OnUnitLoaded(string path)
        {
            try
            {
                var jobHandleParams = Addressables.LoadAssetAsync<UnitParameters>(path);
                await jobHandleParams.Task;
                _units.Add(jobHandleParams.Result);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load unit " + path + " " + e.Message);
            }
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (_units.Count == 0 || _spawnAreaCount == 0 || UnitStack.LevelAndCount.Count == 0)
                return;
            
            var (level, count) = UnitStack.GetFirstUnit();
            var unit = _units[level];

            var randomIndex = Random.Range(0, _spawnAreaCount);
            ref var spawnArea = ref _stashes.SpawnAreaStash.Get(_spawnAreaEntities[randomIndex]);

            var randomCircle = Random.insideUnitCircle * spawnArea.Radius;
            var spawnPosition = spawnArea.RootTransform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
            var spawnRotation = spawnArea.RootTransform.rotation;

            _ = SpawnUtils.SpawnUnit(unit, 1, _stashes, spawnPosition, spawnRotation);
        }

        public void Dispose() { }
    }
}
