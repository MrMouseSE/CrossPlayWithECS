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
            _agentStash = World.GetStash<NavMeshAgentComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var enemyEntity in _enemyFilter)
            {
                ref var agentComponent = ref _agentStash.Get(enemyEntity);
                if (agentComponent.Path != null)
                    agentComponent.NavMeshAgent.SetPath(agentComponent.Path);
            }
        }

        public void Dispose() { }
    }
}