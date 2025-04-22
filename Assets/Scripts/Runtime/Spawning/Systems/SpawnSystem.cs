using System;
using System.Collections.Generic;
using Runtime.Combat.Components;
using Runtime.Movement.Components;
using Runtime.Spawning.Components;
using Runtime.Spawning.Utils;
using Runtime.Unit.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
        
        private Stash<PlayerMarker> _playerMarkerStash;
        private Stash<EnemyMarker> _enemyMarkerStash;
        
        private Stash<AttackComponent> _attackStash;
        private Stash<NavMeshAgentComponent> _navMeshAgentStash;
        private Stash<HealthComponent> _healthStash;
        private Stash<SpawnAreaComponent> _spawnAreaStash;

        private List<Entity> _spawnPlayerAreaEntities;
        private List<Entity> _spawnEnemyAreaEntities;
        
        private SpawnStashes<PlayerMarker> _playerStashes;
        private SpawnStashes<EnemyMarker> _enemyStashes;

        private readonly Dictionary<string, UnitParameters> _units = new();

        public void OnAwake()
        {
            _playerMarkerStash = World.GetStash<PlayerMarker>();
            _enemyMarkerStash = World.GetStash<EnemyMarker>();
            
            _healthStash = World.GetStash<HealthComponent>();
            _spawnAreaStash = World.GetStash<SpawnAreaComponent>();
            _attackStash = World.GetStash<AttackComponent>();
            _navMeshAgentStash = World.GetStash<NavMeshAgentComponent>();
            
            var spawnPlayerAreaFilter = World.Filter.With<SpawnAreaComponent>().With<PlayerMarker>().Build();
            var spawnEnemyAreaFilter = World.Filter.With<SpawnAreaComponent>().With<EnemyMarker>().Build();
            
            SpawnUtils.CacheSpawnAreas(spawnPlayerAreaFilter, ref _spawnPlayerAreaEntities);
            SpawnUtils.CacheSpawnAreas(spawnEnemyAreaFilter, ref _spawnEnemyAreaEntities);

            for (int i = 0; i < SpawnUtils.KeyUnits.Length; i++) 
                OnUnitLoaded(SpawnUtils.KeyUnits[i]);
            

            _playerStashes = new SpawnStashes<PlayerMarker>
            {
                MarkerStash = _playerMarkerStash,
                HealthStash = _healthStash,
                AttackStash = _attackStash,
                NavMeshAgentStash = _navMeshAgentStash,
            };
            
            _enemyStashes = new SpawnStashes<EnemyMarker>
            {
                MarkerStash = _enemyMarkerStash,
                HealthStash = _healthStash,
                AttackStash = _attackStash,
                NavMeshAgentStash = _navMeshAgentStash,
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
                if (unitParams.Faction == UnitFaction.Player)
                {
                    var randomIndex = Random.Range(0, _spawnPlayerAreaEntities.Count);
                    ref var spawnArea = ref _spawnAreaStash.Get(_spawnPlayerAreaEntities[randomIndex]);
                    
                    _ = SpawnUtils.SpawnUnit(unitParams, count, _playerStashes, spawnArea);
                }
                  
                else
                {
                    var randomIndex = Random.Range(0, _spawnEnemyAreaEntities.Count);
                    ref var spawnArea = ref _spawnAreaStash.Get(_spawnEnemyAreaEntities[randomIndex]);
                    
                    _ = SpawnUtils.SpawnUnit(unitParams, count, _enemyStashes, spawnArea);
                }
            }
        }

        public void Dispose() { }
    }
}
