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
    public sealed class FriendlyArrivalSystem : ISystem
    {
        public World World { get; set; }

        private Filter _arrivingUnitsFilter;
        private Stash<NavMeshAgentComponent> _agentStash;
        private Stash<IsMovingToDefensePoint> _isMovingStash;

        public void OnAwake()
        {
            _arrivingUnitsFilter = World.Filter
                .With<PlayerMarker>() 
                .With<NavMeshAgentComponent>()
                .With<IsMovingToDefensePoint>()
                .Build();

            _agentStash = World.GetStash<NavMeshAgentComponent>();
            _isMovingStash = World.GetStash<IsMovingToDefensePoint>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var unitEntity in _arrivingUnitsFilter)
            {
                ref var agentComponent = ref _agentStash.Get(unitEntity);
                var agent = agentComponent.NavMeshAgent;
                
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    _isMovingStash.Remove(unitEntity);
                    agent.isStopped = true;
                    agent.ResetPath();
                }
            }
        }
        public void Dispose() { }
    }
}