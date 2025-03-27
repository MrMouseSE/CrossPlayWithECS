using Runtime.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace Runtime.StaticUtils
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

        public static void SpawnUnit<TMarker>(GameObject unitPrefab, Stash<TMarker> markerStash, 
            Stash<HealthComponent> healthStash, Entity[] spawnAreaEntities, 
            int spawnAreaCount, Stash<SpawnAreaComponent> spawnAreaStash) where TMarker : struct, IComponent
        {
            if (unitPrefab == null || spawnAreaCount == 0)
                return;

            var randomIndex = Random.Range(0, spawnAreaCount);
            ref var spawnArea = ref spawnAreaStash.Get(spawnAreaEntities[randomIndex]);

            var randomCircle = Random.insideUnitCircle * spawnArea.Radius;
            var spawnPosition = spawnArea.RootTransform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
            var spawnRotation = spawnArea.RootTransform.rotation;

            var spawnedUnit = Object.Instantiate(unitPrefab, spawnPosition, spawnRotation);
            var entityUnitProvider = spawnedUnit.GetComponent<EntityProvider>();
            var entityUnit = entityUnitProvider.Entity;

            markerStash.Set(entityUnit, new TMarker());
            healthStash.Set(entityUnit, new HealthComponent { HealthPoints = 100 });
        }
    }
}
