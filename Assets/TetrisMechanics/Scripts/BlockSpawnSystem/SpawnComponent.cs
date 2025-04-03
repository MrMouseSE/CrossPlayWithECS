using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace TetrisMechanics.Scripts.BlockSpawnSystem
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct SpawnComponent : IComponent
    {
        public SpawnPrefabsList SpawnPrefabsList;
        public int CurrentSpawnDificulty;
        public Transform SpawnAnchor;
        public int SpawnHorizontalOffset;
        public int SpawnVerticalOffset;
        public bool RandomSpawnPosition;
        
        [HideInInspector]
        public float UnitSpawnTime;

        public GameObject SpawnPrefab()
        {
            int currentLevel = Mathf.Max(CurrentSpawnDificulty, SpawnPrefabsList.BlockPrefabs.Length);
            Vector3 spawnPos = SpawnAnchor.position + Vector3.up * SpawnVerticalOffset;
            spawnPos += Vector3.right * (RandomSpawnPosition ? Random.Range(0,SpawnHorizontalOffset) : Mathf.FloorToInt((float)SpawnHorizontalOffset/2));
            return Object.Instantiate(SpawnPrefabsList.BlockPrefabs[Random.Range(0,currentLevel)], spawnPos, Quaternion.identity);
        }
    }
}