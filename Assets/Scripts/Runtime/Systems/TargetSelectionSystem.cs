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
    public sealed class TargetSelectionSystem : ISystem
    {
        public World World { get; set; }

        private Filter _unitEnemyFilter;
        private Filter _unitPlayerFilter;
        
        private Stash<UnitComponent> _unitStash;
        private Stash<TargetComponent> _targetStash;
        private Stash<IsNewTargetMarker> _isNewTargetMarkerStash;
        
        private float _systemUpdateTimer = 0f;
        private const float RecalculationInterval = 1.0f;
        private const float PositionEpsilon = 0.1f;
        
        public void OnAwake()
        {
            _unitEnemyFilter = World.Filter.With<UnitComponent>().With<EnemyMarker>().Build();
            _unitPlayerFilter = World.Filter.With<UnitComponent>().With<PlayerMarker>().Build();
            
            _unitStash = World.GetStash<UnitComponent>();
            _targetStash = World.GetStash<TargetComponent>();
            _isNewTargetMarkerStash = World.GetStash<IsNewTargetMarker>();
        }

        public void OnUpdate(float deltaTime) 
        {
            _systemUpdateTimer -= deltaTime;
            if (_systemUpdateTimer > 0) return;
            _systemUpdateTimer = RecalculationInterval;
            
            foreach (var enemyEntity in _unitEnemyFilter)
            {
                ref var enemyUnit = ref _unitStash.Get(enemyEntity);
                ref var target = ref _targetStash.Get(enemyEntity);

                var bestDistance = float.MaxValue;
                var targetPosition = Vector3.zero;
                Entity entityTarget = default;
                var currentEnemyPosition = enemyUnit.RootTransform.position;
                
                foreach (var unitPlayerEntity in _unitPlayerFilter)
                {
                    ref var unitComponent = ref _unitStash.Get(unitPlayerEntity);
                    var targetPos = unitComponent.RootTransform.position;
                    var sqrDist = (currentEnemyPosition - targetPos).sqrMagnitude;
                    if (sqrDist < bestDistance)
                    {
                        bestDistance = sqrDist;
                        targetPosition = targetPos;
                        entityTarget = unitPlayerEntity;
                    }
                }
                if (bestDistance < float.MaxValue && (target.TargetPosition - targetPosition).sqrMagnitude > PositionEpsilon)
                {
                    var direction = (currentEnemyPosition - targetPosition).normalized;
                    if (direction == Vector3.zero)
                        direction = Vector3.forward;

                    target.TargetPosition = targetPosition;
                    target.DirectionToTarget = direction;
                    target.TargetEntity = entityTarget;
                    _isNewTargetMarkerStash.Set(enemyEntity, new IsNewTargetMarker());
                }
            }
        }
        
      
        public void Dispose(){}
    }
}