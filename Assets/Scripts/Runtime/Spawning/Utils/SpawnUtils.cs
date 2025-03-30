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

        public static bool ShouldSpawnUnit(Filter unitFilter, int count)
        {
            int currentCount = 0;
            foreach (var _ in unitFilter)
            {
                currentCount++;
            }
            return currentCount < count;
        }

        public static void SpawnUnit<TMarker>(GameObject unitPrefab, UnitParameters unitParams, SpawnStashes<TMarker> stashes, Entity[] spawnAreaEntities, int spawnAreaCount) where TMarker : struct, IComponent
        {
            var randomIndex = Random.Range(0, spawnAreaCount);
            ref var spawnArea = ref stashes.SpawnAreaStash.Get(spawnAreaEntities[randomIndex]);

            var randomCircle = Random.insideUnitCircle * spawnArea.Radius;
            var spawnPosition = spawnArea.RootTransform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
            var spawnRotation = spawnArea.RootTransform.rotation;

            var spawnedUnit = Object.Instantiate(unitPrefab, spawnPosition, spawnRotation);
            var entityUnitProvider = spawnedUnit.GetComponent<EntityProvider>();
            var entityUnit = entityUnitProvider.Entity;

            stashes.MarkerStash.Set(entityUnit, new TMarker());
            stashes.HealthStash.Set(entityUnit, new HealthComponent { HealthPoints = unitParams.HealthPoints });
            stashes.AttackStash.Set(entityUnit, new AttackComponent { Damage = unitParams.Damage, AttackRange = Random.Range(unitParams.AttackRange.x, unitParams.AttackRange.y), 
                AttackCooldown = Random.Range(unitParams.AttackCooldown.x, unitParams.AttackCooldown.y) });
            stashes.NavMeshAgentStash.Get(entityUnit).NavMeshAgent.speed = Random.Range(unitParams.Speed.x, unitParams.Speed.y);
            stashes.NavMeshAgentStash.Get(entityUnit).StoppingDistance = Random.Range(unitParams.StoppingDistance.x, unitParams.StoppingDistance.y);
        }
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
