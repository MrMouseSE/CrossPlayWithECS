using UnityEngine;

namespace Runtime.Components
{
    [CreateAssetMenu(fileName = "UnitParameters", menuName = "Scriptable Objects/UnitParameters")]
    public class UnitParameters : ScriptableObject
    {
        public float Damage;
        public float AttackRange;
        public int HealthPoints;
        public float AttackCooldown;
    }
}
