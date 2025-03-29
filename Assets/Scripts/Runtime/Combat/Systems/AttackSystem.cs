using Runtime.Combat.Components;
using Runtime.Movement.Components;
using Runtime.Targeting.Components;
using Runtime.Unit.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Runtime.Combat.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class AttackSystem : ISystem
    {
        public World World { get; set; }

        private Filter _unitFilter;

        private Stash<NavMeshAgentComponent> _agentStash;
        private Stash<AttackComponent> _attackStash;
        private Stash<TargetComponent> _targetStash;
        private Stash<HealthComponent> _healthStash;

        public void OnAwake()
        {
            _unitFilter = World.Filter.With<NavMeshAgentComponent>().With<AttackComponent>().With<TargetComponent>().Build();
            _agentStash = World.GetStash<NavMeshAgentComponent>();
            _attackStash = World.GetStash<AttackComponent>();
            _targetStash = World.GetStash<TargetComponent>();
            _healthStash = World.GetStash<HealthComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var unitEntity in _unitFilter)
            {
                ref var agentComponent = ref _agentStash.Get(unitEntity);
                ref var attackComponent = ref _attackStash.Get(unitEntity);
                ref var targetComponent = ref _targetStash.Get(unitEntity);

                attackComponent.CurrentCooldown -= deltaTime;
                if (agentComponent.NavMeshAgent.remainingDistance < agentComponent.NavMeshAgent.stoppingDistance && targetComponent.TargetEntity != default)
                {
                    if (attackComponent.CurrentCooldown <= 0)
                    {
                        bool isDisposed = World.IsDisposed(targetComponent.TargetEntity);
                        if (!isDisposed)
                        {
                            ref var healthComponent = ref _healthStash.Get(targetComponent.TargetEntity);
                            healthComponent.HealthPoints -= attackComponent.Damage;
                            attackComponent.CurrentCooldown = attackComponent.AttackCooldown;   
                        }
                    }
                }
            }
        }

        public void Dispose() { }
    }
}