using System.Collections.Generic;
using Runtime.Movement.Components;
using Runtime.Unit.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Runtime.Unit.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DefensePointAssignmentSystem : ISystem
    {
        public World World { get; set; }

        private Filter _newFriendlyUnitsFilter;
        private Filter _availableDefensePointsFilter;

        private Stash<PlayerMarker> _playerMarkerStash;
        private Stash<UnitComponent> _unitStash;
        private Stash<AssignedDefensePoint> _assignedPointStash;
        private Stash<IsMovingToDefensePoint> _isMovingStash;
        private Stash<IsOccupied> _isOccupiedStash;
        private Entity _defaultDefensePointEntity = default;

        public void OnAwake()
        {
            _newFriendlyUnitsFilter = World.Filter
                .With<PlayerMarker>()
                .With<UnitComponent>()
                .With<NavMeshAgentComponent>()
                .Without<AssignedDefensePoint>()
                .Build();


            _availableDefensePointsFilter = World.Filter.With<DefensePointComponent>().With<UnitComponent>()
                .Without<IsOccupied>().Build();

            _unitStash = World.GetStash<UnitComponent>();
            _assignedPointStash = World.GetStash<AssignedDefensePoint>();
            _isMovingStash = World.GetStash<IsMovingToDefensePoint>();
            _isOccupiedStash = World.GetStash<IsOccupied>();

            _defaultDefensePointEntity = World.Filter.With<DefensePointComponent>().With<IsDefaultDefensePoint>()
                .Build().First();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var unitEntity in _newFriendlyUnitsFilter)
            {
                ref var unitComponent = ref _unitStash.Get(unitEntity);
                var unitPosition = unitComponent.RootTransform.position;

                Entity bestPointEntity = default;
                var minSqrDistance = float.MaxValue;

                foreach (var pointEntity in _availableDefensePointsFilter)
                {
                    ref var pointUnitComponent = ref _unitStash.Get(pointEntity);
                    var pointPosition = pointUnitComponent.RootTransform.position;

                    var sqrDistance = (unitPosition - pointPosition).sqrMagnitude;
                    if (sqrDistance < minSqrDistance)
                    {
                        minSqrDistance = sqrDistance;
                        bestPointEntity = pointEntity;
                    }
                }

                Entity assignedEntity;
                if (bestPointEntity != default)
                {
                    assignedEntity = bestPointEntity;
                    _isOccupiedStash.Add(assignedEntity);
                    Debug.Log($"Unit {unitEntity.Id} assigned to point {assignedEntity.Id}");
                }
                else
                {
                    assignedEntity = _defaultDefensePointEntity;
                    Debug.Log($"Unit {unitEntity.Id} assigned to DEFAULT point {assignedEntity.Id}");
                }

                _assignedPointStash.Set(unitEntity, new AssignedDefensePoint { TargetPointEntity = assignedEntity });
                _isMovingStash.Add(unitEntity);
            }
        }

        public void Dispose() { }
    }
}