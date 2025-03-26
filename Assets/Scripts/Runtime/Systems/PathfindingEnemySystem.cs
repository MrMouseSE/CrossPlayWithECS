using Runtime.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
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

        private Filter _enemyFilter;
        private Filter _playerFilter;
        private Stash<NavMeshAgentComponent> _navAgentStash;
        private Stash<UnitComponent> _unitStash;

        private const float TargetReachedThreshold = 2f;
        private const float StopDistance = 4.0f;
        private NavMeshPath _path = new();

        public void OnAwake()
        {
            _enemyFilter = World.Filter.With<EnemyMarker>().With<UnitComponent>().Build();
            _playerFilter = World.Filter.With<PlayerMarker>().With<UnitComponent>().Build();

            _navAgentStash = World.GetStash<NavMeshAgentComponent>();
            _unitStash = World.GetStash<UnitComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var enemyEntity in _enemyFilter)
            {
                ref var agentComponent = ref _navAgentStash.Get(enemyEntity);
                ref var unitEnemyComponent = ref _unitStash.Get(enemyEntity);

                var sqrDistToTarget = (unitEnemyComponent.RootTransform.position - agentComponent.TargetPosition).sqrMagnitude;
                // if (sqrDistToTarget > TargetReachedThreshold * TargetReachedThreshold)
                // {
                //     DrawPath(agentComponent.NavMeshAgent);
                //     continue;
                // }

                var bestSqrDistance = float.MaxValue;
                var targetPosition = Vector3.zero;
                var currentEnemyPosition = unitEnemyComponent.RootTransform.position;


                foreach (var targetEntity in _playerFilter)
                {
                    ref var unitComponent = ref _unitStash.Get(targetEntity);
                    var targetPos = unitComponent.RootTransform.position;
                    var sqrDist = (currentEnemyPosition - targetPos).sqrMagnitude;
                    if (sqrDist < bestSqrDistance)
                    {
                        bestSqrDistance = sqrDist;
                        targetPosition = targetPos;
                    }
                }

                if (bestSqrDistance < float.MaxValue)
                {
                    var direction = (unitEnemyComponent.RootTransform.position - targetPosition).normalized;
                    if (direction == Vector3.zero)
                        direction = Vector3.forward;


                    agentComponent.TargetPosition = targetPosition + direction * StopDistance;
                    //agentComponent.PathCalculated = true;

                    if (agentComponent.NavMeshAgent.CalculatePath(agentComponent.TargetPosition, _path))
                    {
                        agentComponent.Path = _path;
                    }
                    
                    Debug.Log("Computed path to " + agentComponent.TargetPosition);
                }
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