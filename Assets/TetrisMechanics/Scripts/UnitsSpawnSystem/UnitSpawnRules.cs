using UnityEngine;

namespace TetrisMechanics.Scripts.UnitsSpawnSystem
{
    [CreateAssetMenu(fileName = "UnitSpawnRules", menuName = "Scriptable Objects/UnitSpawnRules")]
    public class UnitSpawnRules : ScriptableObject
    {
        public float AddUnitPercent;
        public float PauseTimeForSpawn;
    }
}
