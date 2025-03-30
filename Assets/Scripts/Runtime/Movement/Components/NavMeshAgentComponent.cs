using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.AI;

namespace Runtime.Movement.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct NavMeshAgentComponent : IComponent
    {
        public NavMeshAgent NavMeshAgent;
        public NavMeshPath Path;
        public float StoppingDistance;
    }
}