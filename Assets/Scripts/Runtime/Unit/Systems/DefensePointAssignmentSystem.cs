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

            _availableDefensePointsFilter = World.Filter
                .With<DefensePointComponent>()
                .With<UnitComponent>()
                .Without<IsOccupied>()
                .Without<IsDefaultDefensePoint>()
                .Build();
            
            _defaultDefensePointEntity = World.Filter
                .With<DefensePointComponent>()
                .With<IsDefaultDefensePoint>()
                .Build()
                .First();
            
            _assignedPointStash = World.GetStash<AssignedDefensePoint>();
            _isMovingStash = World.GetStash<IsMovingToDefensePoint>();
            _isOccupiedStash = World.GetStash<IsOccupied>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var unitEntity in _newFriendlyUnitsFilter)
            {
                var bestPointEntity = _defaultDefensePointEntity;

                foreach (var pointEntity in _availableDefensePointsFilter)
                {
                    bestPointEntity = pointEntity;
                    break;
                }

                if (bestPointEntity != _defaultDefensePointEntity)
                    _isOccupiedStash.Add(bestPointEntity);

                _assignedPointStash.Set(unitEntity, new AssignedDefensePoint { TargetPointEntity = bestPointEntity });
                _isMovingStash.Add(unitEntity);
            }
        }

        public void Dispose()
        {
        }
    }
}