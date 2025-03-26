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
    public sealed class MovementSystem : ISystem
    {
        public World World { get; set; }

        private Filter _enemyFilter;
        private Filter _playerTargetFilter;
        private Stash<NavMeshAgentComponent> _agentStash;
        private Stash<UnitComponent> _unitStash;
        
       

        public void OnAwake()
        {
            _enemyFilter = World.Filter.With<NavMeshAgentComponent>().With<EnemyMarker>().Build();
            //_playerTargetFilter = World.Filter.With<PlayerMarker>().With<UnitComponent>().Build();

            _agentStash = World.GetStash<NavMeshAgentComponent>();
            //_unitStash = World.GetStash<UnitComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var enemyEntity in _enemyFilter)
            {
                ref var agentComponent = ref _agentStash.Get(enemyEntity);
               // ref var unitEnemyComponent = ref _unitStash.Get(enemyEntity);
                agentComponent.NavMeshAgent.SetPath(agentComponent.Path);
                //
                // if (agentComponent.PathCalculated)
                // {
                //     var sqrDistToTarget = (unitEnemyComponent.RootTransform.position - agentComponent.TargetPosition)
                //         .sqrMagnitude;
                //     if (sqrDistToTarget > TargetReachedThreshold * TargetReachedThreshold)
                //     {
                //         DrawPath(agentComponent.NavMeshAgent);
                //         continue;
                //     }
                // }
                //
                // var bestSqrDistance = float.MaxValue;
                // var bestTargetPos = Vector3.zero;
                // var enemyPos = unitEnemyComponent.RootTransform.position;
                //
                //
                // foreach (var targetEntity in _playerTargetFilter)
                // {
                //     ref var unitComponent = ref _unitStash.Get(targetEntity);
                //     var targetPos = unitComponent.RootTransform.position;
                //     var sqrDist = (enemyPos - targetPos).sqrMagnitude;
                //     if (sqrDist < bestSqrDistance)
                //     {
                //         bestSqrDistance = sqrDist;
                //         bestTargetPos = targetPos;
                //     }
                // }
                //
                // if (bestSqrDistance < float.MaxValue)
                // {
                //     var direction = (unitEnemyComponent.RootTransform.position - bestTargetPos).normalized;
                //     if (direction == Vector3.zero)
                //         direction = Vector3.forward;
                //
                //
                //     agentComponent.TargetPosition = bestTargetPos + direction * StopDistance;
                //     agentComponent.PathCalculated = true;
                //
                //     if (agentComponent.NavMeshAgent.CalculatePath(agentComponent.TargetPosition, _path))
                //     {
                //         agentComponent.NavMeshAgent.SetPath(_path);
                //     }
                // }
            }
        }
        
        public void Dispose()
        {
        }
    }
}