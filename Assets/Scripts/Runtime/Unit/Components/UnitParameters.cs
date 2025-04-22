using Scellecs.Morpeh.Providers;
using Unity.Mathematics;
using UnityEngine;

namespace Runtime.Unit.Components
{
    [CreateAssetMenu(fileName = "UnitParameters", menuName = "Scriptable Objects/UnitParameters")]
    public class UnitParameters : ScriptableObject
    {
        public EntityProvider UnitPrefab;
        public UnitFaction Faction;
        public float2 Damage;
        public float2 AttackRange;
        public float2 HealthPoints;
        public float2 AttackCooldown;
        public float2 Speed;
        public float2 StoppingDistance;
    }
    public enum UnitFaction
    {
        Player,
        Enemy
    }
}
