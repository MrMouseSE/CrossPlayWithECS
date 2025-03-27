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

        private Filter _unitFilter;
        private Filter _playerTargetFilter;
        private Stash<NavMeshAgentComponent> _agentStash;
        private Stash<UnitComponent> _unitStash;
        private Stash<IsNewTargetMarker> _isNewTargetStash;

        public void OnAwake()
        {
            _unitFilter = World.Filter.With<NavMeshAgentComponent>().With<IsNewTargetMarker>().Build();
            _agentStash = World.GetStash<NavMeshAgentComponent>();
            _isNewTargetStash = World.GetStash<IsNewTargetMarker>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var unitEntity in _unitFilter)
            {
                ref var agentComponent = ref _agentStash.Get(unitEntity);
                agentComponent.NavMeshAgent.SetPath(agentComponent.Path);
                _isNewTargetStash.Remove(unitEntity);
            }
        }

        public void Dispose() { }
    }
}