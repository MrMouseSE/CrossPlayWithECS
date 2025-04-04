using Runtime.Combat.Components;
using Runtime.Movement.Components;
using Runtime.Targeting.Components;
using Runtime.Unit.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace Runtime.Movement.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PathfindingTargetSystem : ISystem
    {
        public World World { get; set; }

      
        private Filter _unitsWithPathRequestFilter;
        private Filter _debugDrawFilter;

        private Stash<NavMeshAgentComponent> _navAgentStash;
        private Stash<TargetComponent> _targetStash;
        

        public void OnAwake()
        {
            _unitsWithPathRequestFilter = World.Filter
                .With<NavMeshAgentComponent>()
                .With<TargetComponent>()
                .With<IsNewTargetMarker>()
                .Without<AssignedDefensePoint>()
                .Build();
            _debugDrawFilter = World.Filter.With<NavMeshAgentComponent>().Build();

            _navAgentStash = World.GetStash<NavMeshAgentComponent>();
            _targetStash = World.GetStash<TargetComponent>();
            
        }

        public void OnUpdate(float deltaTime)
        {
            #if UNITY_EDITOR
            foreach (var unitEntity in _debugDrawFilter)
            {
                ref var agentComponent = ref _navAgentStash.Get(unitEntity);
                if (agentComponent.Path == null) continue;
                DrawPath(agentComponent.Path);
            }
            #endif

            foreach (var unitEntity in _unitsWithPathRequestFilter)
            {
                ref var agentComponent = ref _navAgentStash.Get(unitEntity);
                ref var targetComponent = ref _targetStash.Get(unitEntity);
               
                if (targetComponent.TargetEntity != default && !World.IsDisposed(targetComponent.TargetEntity))
                {
                    var path = new NavMeshPath();
                    var targetPoint = targetComponent.TargetPosition;
            
                    if (agentComponent.NavMeshAgent.CalculatePath(targetPoint, path))
                    {
                        agentComponent.NavMeshAgent.stoppingDistance = agentComponent.StoppingDistance;
                        agentComponent.Path = path;
                    }
                    else 
                    {
                        Debug.LogError($"FAILED to calculate path for unit {unitEntity.Id} from {agentComponent.NavMeshAgent.transform.position} to target {targetPoint}.");
                    }
                }
            }
        }

        private static void DrawPath(NavMeshPath path)
        {
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
            }
        }

        public void Dispose() { }
    }
}
