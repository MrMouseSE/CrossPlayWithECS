using Runtime.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Runtime.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class NavigationSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filterAgentComponent;
       
        private Stash<NavMeshAgentComponent> _agentStash;
        private Transform _targetTransform;
        
        public void OnAwake()
        {
            _filterAgentComponent = World.Filter.With<NavMeshAgentComponent>().Build();
            _agentStash = World.GetStash<NavMeshAgentComponent>();
            
            var entityGameData = World.Filter.With<GeneralGameDataComponent>().Build().First();
            var stashGameData = World.GetStash<GeneralGameDataComponent>();
            ref var entityComponent = ref stashGameData.Get(entityGameData);
            _targetTransform = entityComponent.TestTarget;
        }

        public void OnUpdate(float deltaTime)
        {
            // foreach (var entity in _filterAgentComponent) {
            //     ref var agentComponent = ref _agentStash.Get(entity);
            //     agentComponent.NavMeshAgent.destination = _targetTransform.position;
            // }
        }

        public void Dispose()
        {
        }
    }
}