using System.Collections.Generic;
using System.Threading.Tasks;
using Runtime.Combat.Components;
using Runtime.Movement.Components;
using Runtime.Spawning.Components;
using Runtime.Unit.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace Runtime.Spawning.Utils
{
    public static class SpawnUtils
    {
        public static void CacheSpawnAreas(Filter spawnAreaFilter, ref List<Entity> spawnAreaEntities)
        {
            spawnAreaEntities = new List<Entity>();

            foreach (var entity in spawnAreaFilter) 
                spawnAreaEntities.Add(entity);
        }
        
        public static async Task SpawnUnit<TMarker>(UnitParameters unitParams,int count, SpawnStashes<TMarker> stashes, SpawnAreaComponent spawnArea) where TMarker : struct, IComponent
        {
            var spawnedUnit = Object.InstantiateAsync<EntityProvider>(unitParams.UnitPrefab, count);
            await spawnedUnit;

            for (int i = 0; i < count; i++)
            {
                var unit = spawnedUnit.Result[i].Entity;
                var transform = spawnedUnit.Result[i].transform;
                SetPositionAndRotation(spawnArea,transform);
                SetData(unitParams, stashes, unit);
            }
        }

        private static void SetPositionAndRotation(SpawnAreaComponent spawnArea, Transform transform)
        {
            var randomCircle = Random.insideUnitCircle * spawnArea.Radius;
            var spawnPosition = spawnArea.RootTransform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
            var spawnRotation = spawnArea.RootTransform.rotation;
            
            transform.SetPositionAndRotation(spawnPosition, spawnRotation);
        }

        private static void SetData<TMarker>(UnitParameters unitParams, SpawnStashes<TMarker> stashes, Entity unit) where TMarker : struct, IComponent
        {
            stashes.MarkerStash.Set(unit, new TMarker());
            stashes.HealthStash.Set(unit, new HealthComponent
            {
                HealthPoints = Random.Range(unitParams.HealthPoints.x, unitParams.HealthPoints.y) 
            });
            stashes.AttackStash.Set(unit, new AttackComponent 
            { 
                Damage = Random.Range(unitParams.Damage.x, unitParams.Damage.y),
                AttackRange = Random.Range(unitParams.AttackRange.x, unitParams.AttackRange.y), 
                AttackCooldown = Random.Range(unitParams.AttackCooldown.x, unitParams.AttackCooldown.y) 
            });
            stashes.NavMeshAgentStash.Get(unit).NavMeshAgent.speed = Random.Range(unitParams.Speed.x, unitParams.Speed.y);
            stashes.NavMeshAgentStash.Get(unit).StoppingDistance = Random.Range(unitParams.StoppingDistance.x, unitParams.StoppingDistance.y);
        }
        
        
        public static readonly string[] KeyUnits =
        {
            "UnitPlayerCommon", "UnitPlayerEpic", 
            "UnitPlayerLegendary", "UnitPlayerMythic",
            "UnitPlayerRare", "UnitPlayerUncommon",
            
            "UnitEnemyCommon", "UnitEnemyEpic", 
            "UnitEnemyLegendary", "UnitEnemyMythic",
            "UnitEnemyRare", "UnitEnemyUncommon"
        };
    }

    public struct SpawnStashes<TMarker> where TMarker : struct, IComponent
    {
        public Stash<TMarker> MarkerStash;
        public Stash<HealthComponent> HealthStash;
        public Stash<AttackComponent> AttackStash;
        public Stash<NavMeshAgentComponent> NavMeshAgentStash;
    }
    
}
