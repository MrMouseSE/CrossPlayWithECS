using Runtime.Targeting.Components;
using Runtime.Unit.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Runtime.Targeting.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TargetSelectionSystem : ISystem
    {
        public World World { get; set; }

        private Filter _unitEnemyFilter;
        private Filter _unitPlayerFilter;

        private Stash<UnitComponent> _unitStash;
        private Stash<TargetComponent> _targetStash;
        private Stash<IsNewTargetMarker> _isNewTargetMarkerStash;
        
        private float _systemUpdateTimer = 0f;
        private const float RecalculationInterval = 0.5f;
        private const float PositionEpsilonSqr = 0.1f * 0.1f;

        public void OnAwake()
        {
            _unitEnemyFilter = World.Filter.With<UnitComponent>().With<EnemyMarker>().With<HealthComponent>().Build();
            _unitPlayerFilter = World.Filter.With<UnitComponent>().With<PlayerMarker>().With<HealthComponent>().Build();

            _unitStash = World.GetStash<UnitComponent>();
            _targetStash = World.GetStash<TargetComponent>();
            _isNewTargetMarkerStash = World.GetStash<IsNewTargetMarker>();
        }

        public void OnUpdate(float deltaTime)
        {
            _systemUpdateTimer -= deltaTime;
            if (_systemUpdateTimer > 0) return;
            _systemUpdateTimer = RecalculationInterval;

            SelectTargetsForEnemies();
            SelectTargetsForPlayers();
        }

        private void SelectTargetsForEnemies()
        {
            foreach (var enemyEntity in _unitEnemyFilter)
            {
                FindAndAssignNearestTarget(enemyEntity, _unitPlayerFilter);
            }
        }

        private void SelectTargetsForPlayers()
        {
            foreach (var playerEntity in _unitPlayerFilter)
            {
                FindAndAssignNearestTarget(playerEntity, _unitEnemyFilter);
            }
        }

        private void FindAndAssignNearestTarget(Entity searchingUnitEntity, Filter potentialTargetsFilter)
        {
            ref var searchingUnit = ref _unitStash.Get(searchingUnitEntity);
            var currentUnitPosition = searchingUnit.RootTransform.position;

            Entity bestTargetEntity = default;
            var bestTargetPosition = Vector3.zero;
            var bestSqrDistance = float.MaxValue;


            foreach (var potentialTargetEntity in potentialTargetsFilter)
            {
                ref var targetUnitComponent = ref _unitStash.Get(potentialTargetEntity);
                var targetPos = targetUnitComponent.RootTransform.position;
                var sqrDist = (currentUnitPosition - targetPos).sqrMagnitude;
                
                if (sqrDist < bestSqrDistance)
                {
                    bestSqrDistance = sqrDist;
                    bestTargetPosition = targetPos;
                    bestTargetEntity = potentialTargetEntity;
                }
            }
            
            ref var target = ref _targetStash.Get(searchingUnitEntity);
            
            if (target.TargetEntity != bestTargetEntity || (target.TargetPosition - bestTargetPosition).sqrMagnitude > PositionEpsilonSqr)
            {
                target.TargetEntity = bestTargetEntity;
                target.TargetPosition = bestTargetPosition;
                target.DirectionToTarget = (bestTargetPosition - currentUnitPosition).normalized;
                if (target.DirectionToTarget == Vector3.zero)
                    target.DirectionToTarget = searchingUnit.RootTransform.forward;
                
                _isNewTargetMarkerStash.Set(searchingUnitEntity);
            }
        }

        public void Dispose() { }
    }
}