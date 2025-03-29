using Runtime.Movement.Components;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;
using UnityEditor;
using UnityEngine.AI;

namespace Runtime.Movement.Providers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class NavMeshAgentProvider : MonoProvider<NavMeshAgentComponent>
    {
#if UNITY_EDITOR
        private NavMeshAgent _agent;
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void OnDrawGizmos()
        {
            Handles.Label(transform.position, _agent.remainingDistance.ToString());
        }  
#endif
    }
}