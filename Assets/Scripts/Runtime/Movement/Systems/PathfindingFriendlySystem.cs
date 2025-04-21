using Runtime.Movement.Components;
using Runtime.Targeting.Components;
using Runtime.Unit.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.AI;

namespace Runtime.Movement.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PathfindingFriendlySystem  : ISystem
    {
        public World World { get; set; }

        private Filter _friendlyMovingToPointFilter;

        private Stash<NavMeshAgentComponent> _navAgentStash;
        private Stash<AssignedDefensePoint> _assignedPointStash;
        private Stash<UnitComponent> _unitStash;
        private Stash<IsNewTargetMarker> _isNewTargetStash;

        public void OnAwake()
        {
            _friendlyMovingToPointFilter = World.Filter
                .With<PlayerMarker>()
                .With<NavMeshAgentComponent>()
                .With<AssignedDefensePoint>()
                .With<IsMovingToDefensePoint>() 
                .Without<IsNewTargetMarker>() 
                .Build();

            _navAgentStash = World.GetStash<NavMeshAgentComponent>();
            _assignedPointStash = World.GetStash<AssignedDefensePoint>();
            _unitStash = World.GetStash<UnitComponent>();
            _isNewTargetStash = World.GetStash<IsNewTargetMarker>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var unitEntity in _friendlyMovingToPointFilter)
            {
                ref var agentComponent = ref _navAgentStash.Get(unitEntity);
                ref var assignedPoint = ref _assignedPointStash.Get(unitEntity);
                ref var pointUnitComponent = ref _unitStash.Get(assignedPoint.TargetPointEntity);
                
                var targetPosition = pointUnitComponent.RootTransform.position;
                var path = new NavMeshPath();
                if (agentComponent.NavMeshAgent.CalculatePath(targetPosition, path))
                {
                    agentComponent.Path = path;
                    _isNewTargetStash.Set(unitEntity); 
                }
            }
        }

        public void Dispose() { }
    }
}
