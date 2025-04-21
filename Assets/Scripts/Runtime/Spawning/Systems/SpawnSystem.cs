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
    public sealed class SpawnSystem : ISystem
    {
        public World World { get; set; }
        
        private Filter _spawnAreaFilter;
        
        private Stash<AttackComponent> _attackStash;
        private Stash<NavMeshAgentComponent> _navMeshAgentStash;
        private Stash<PlayerMarker> _playerMarkerStash;
        private Stash<EnemyMarker> _enemyMarkerStash;
        private Stash<HealthComponent> _healthStash;
        private Stash<SpawnAreaComponent> _spawnAreaStash;
        private Entity _playerUnitCountEntity;

        private Entity[] _spawnAreaEntities;
        private int _spawnAreaCount;
        private readonly Dictionary<string,UnitParameters> _units = new();
        private SpawnStashes<PlayerMarker> _playerStashes;
        private SpawnStashes<EnemyMarker> _enemyStashes;


        public void OnAwake()
        {
            _playerMarkerStash = World.GetStash<PlayerMarker>();
            _enemyMarkerStash = World.GetStash<EnemyMarker>();
            _healthStash = World.GetStash<HealthComponent>();
            _spawnAreaStash = World.GetStash<SpawnAreaComponent>();
            _attackStash = World.GetStash<AttackComponent>();
            _navMeshAgentStash = World.GetStash<NavMeshAgentComponent>();
            _spawnAreaFilter = World.Filter.With<SpawnAreaComponent>().With<PlayerMarker>().Build();
            
            SpawnUtils.CacheSpawnAreas(_spawnAreaFilter, ref _spawnAreaEntities, ref _spawnAreaCount);

            for (int i = 0; i < SpawnUtils.KeyUnits.Length; i++) 
                OnUnitLoaded(SpawnUtils.KeyUnits[i]);
            

            _playerStashes = new SpawnStashes<PlayerMarker>
            {
                MarkerStash = _playerMarkerStash,
                HealthStash = _healthStash,
                AttackStash = _attackStash,
                NavMeshAgentStash = _navMeshAgentStash,
                SpawnAreaStash = _spawnAreaStash
            };
            
            _enemyStashes = new SpawnStashes<EnemyMarker>
            {
                MarkerStash = _enemyMarkerStash,
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
                _units.Add(path, jobHandleParams.Result);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load unit " + path + " " + e.Message);
            }
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (UnitStack.IdAndCount.Count == 0 || _units.Count < SpawnUtils.KeyUnits.Length)
                return;
            
            var (id, count) = UnitStack.GetFirstUnit();
            
            if (_units.TryGetValue(id, out var unitParams))
            {
                var randomIndex = Random.Range(0, _spawnAreaCount);
                ref var spawnArea = ref _playerStashes.SpawnAreaStash.Get(_spawnAreaEntities[randomIndex]);

                var randomCircle = Random.insideUnitCircle * spawnArea.Radius;
                var spawnPosition = spawnArea.RootTransform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
                var spawnRotation = spawnArea.RootTransform.rotation;

                _ = SpawnUtils.SpawnUnit(unitParams, count, _playerStashes, spawnPosition, spawnRotation);
            }
        }

        public void Dispose() { }
    }
}
