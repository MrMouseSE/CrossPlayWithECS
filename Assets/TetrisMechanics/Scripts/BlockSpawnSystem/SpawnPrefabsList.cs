using UnityEngine;

namespace TetrisMechanics.Scripts.BlockSpawnSystem
{
    [CreateAssetMenu(fileName = "SpawnPrefabsList", menuName = "Scriptable Objects/SpawnPrefabsList")]
    public class SpawnPrefabsList : ScriptableObject
    {
        public GameObject[] BlockPrefabs;
    }
}