using Runtime.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Runtime.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PathfindingEnemySystem : ISystem
    {
        public World World { get; set; }

        private Filter _unitFilter;
        private Filter _testFilter;
        private Stash<NavMeshAgentComponent> _navAgentStash;
        private Stash<TargetComponent> _targetStash;

        public void OnAwake()
        {
            _unitFilter = World.Filter.With<NavMeshAgentComponent>().With<TargetComponent>().With<IsNewTargetMarker>().Build();
            _testFilter = World.Filter.With<NavMeshAgentComponent>().With<TargetComponent>().Build();

            _navAgentStash = World.GetStash<NavMeshAgentComponent>();
            _targetStash = World.GetStash<TargetComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
#if UNITY_EDITOR
            foreach (var unitEntity in _testFilter)
            {
                ref var agentComponent = ref _navAgentStash.Get(unitEntity);
                DrawPath(agentComponent.NavMeshAgent);
            }      
#endif
            
            foreach (var unitEntity in _unitFilter)
            {
                ref var agentComponent = ref _navAgentStash.Get(unitEntity);
                ref var targetComponent = ref _targetStash.Get(unitEntity);
                
                var path = new NavMeshPath();
                var targetPoint = targetComponent.TargetPosition + targetComponent.DirectionToTarget;
                if (agentComponent.NavMeshAgent.CalculatePath(targetPoint, path)) 
                    agentComponent.Path = path;
            }
        }

        private static void DrawPath(NavMeshAgent agent)
        {
            if (agent.hasPath)
            {
                var path = agent.path;
                for (int i = 0; i < path.corners.Length - 1; i++)
                {
                    Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
                }
            }
        }

        public void Dispose() { }
    }
}
