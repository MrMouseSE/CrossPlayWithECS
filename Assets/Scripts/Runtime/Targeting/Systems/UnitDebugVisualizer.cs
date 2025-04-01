#if UNITY_EDITOR
using UnityEngine;

namespace Runtime.Targeting.Systems
{
    public class UnitDebugVisualizer : MonoBehaviour
    {
        public float AttackRadius = 1.0f;
        public Vector3 TargetPosition = Vector3.zero;
        public bool HasTarget = false;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, AttackRadius);
            
            if (HasTarget && (transform.position - TargetPosition).sqrMagnitude <= AttackRadius * AttackRadius)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position + Vector3.up * 1.25f, TargetPosition + Vector3.up * 1.25f);
            }
        }
    }
}
#endif