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
        public static void CacheSpawnAreas(Filter spawnAreaFilter, ref Entity[] spawnAreaEntities, ref int spawnAreaCount)
        {
            spawnAreaCount = 0;
            spawnAreaEntities = new Entity[16];

            foreach (var entity in spawnAreaFilter)
            {
                if (spawnAreaCount >= spawnAreaEntities.Length)
                {
                    System.Array.Resize(ref spawnAreaEntities, spawnAreaEntities.Length * 2);
                }
                spawnAreaEntities[spawnAreaCount++] = entity;
            }
        }
        
        public static async Task SpawnUnit<TMarker>(UnitParameters unitParams,int count, SpawnStashes<TMarker> stashes,Vector3 spawnPosition, Quaternion spawnRotation) where TMarker : struct, IComponent
        {
            var spawnedUnit = Object.InstantiateAsync<EntityProvider>(unitParams.UnitPrefab, count, spawnPosition, spawnRotation);
            await spawnedUnit;

            for (int i = 0; i < count; i++)
            {
                var unit = spawnedUnit.Result[i].Entity;
                SetData(unitParams, stashes, unit);
            }
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
        public Stash<SpawnAreaComponent> SpawnAreaStash;
        public Stash<NavMeshAgentComponent> NavMeshAgentStash;
    }
    
}
