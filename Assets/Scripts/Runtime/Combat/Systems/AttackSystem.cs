using Runtime.Combat.Components;
using Runtime.Movement.Components;
using Runtime.Targeting.Components;
using Runtime.Unit.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Runtime.Combat.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AttackSystem : ISystem
    {
        public World World { get; set; }

        private Filter _attackFilter;
        
        private Stash<AttackComponent> _attackStash;
        private Stash<TargetComponent> _targetStash;
        private Stash<HealthComponent> _healthStash;
        private Stash<UnitComponent> _unitStash;

        public void OnAwake()
        {
            _attackFilter = World.Filter
                .With<UnitComponent>()
                .With<AttackComponent>()
                .With<HealthComponent>()
                .With<TargetComponent>()
                .Build();


            _attackStash = World.GetStash<AttackComponent>();
            _targetStash = World.GetStash<TargetComponent>();
            _healthStash = World.GetStash<HealthComponent>();
            _unitStash = World.GetStash<UnitComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var unitEntity in _attackFilter)
            {
                ref var attackComponent = ref _attackStash.Get(unitEntity);
                ref var targetComponent = ref _targetStash.Get(unitEntity);
                ref var unitComponent = ref _unitStash.Get(unitEntity);

                attackComponent.CurrentCooldown -= deltaTime;

                if (targetComponent.TargetEntity != default && !World.IsDisposed(targetComponent.TargetEntity))
                {
                    var unitPosition = unitComponent.RootTransform.position;
                    var targetPosition = targetComponent.TargetPosition;

                    var sqrDistanceToTarget = (unitPosition - targetPosition).sqrMagnitude;
                    var attackRangeSqr = attackComponent.AttackRange * attackComponent.AttackRange;

                    if (sqrDistanceToTarget <= attackRangeSqr && attackComponent.CurrentCooldown <= 0)
                    {
                        ref var targetHealthComponent = ref _healthStash.Get(targetComponent.TargetEntity);
                        targetHealthComponent.HealthPoints -= attackComponent.Damage;
                        attackComponent.CurrentCooldown = attackComponent.AttackCooldown;
                    }
                }
            }
        }

        public void Dispose() { }
    }
}