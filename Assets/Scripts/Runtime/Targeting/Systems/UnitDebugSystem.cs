using Runtime.Combat.Components;
using Runtime.Targeting.Components;
using Runtime.Unit.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Runtime.Targeting.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UnitDebugSystem : ISystem
    {
        public World World { get; set; }

        private Filter _debugUnitFilter;
        private Stash<UnitComponent> _unitStash;
        private Stash<AttackComponent> _attackStash;
        private Stash<TargetComponent> _targetStash;

        public void OnAwake()
        {
            _debugUnitFilter = World.Filter
                .With<UnitComponent>()
                .With<AttackComponent>()
                .With<TargetComponent>()
                .Build();

            _unitStash = World.GetStash<UnitComponent>();
            _attackStash = World.GetStash<AttackComponent>();
            _targetStash = World.GetStash<TargetComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _debugUnitFilter)
            {
                if (!_unitStash.Has(entity)) continue;
                ref var unitComponent = ref _unitStash.Get(entity);
                if (unitComponent.RootGameObject == null) continue;

                var visualizer = unitComponent.RootGameObject.GetComponent<UnitDebugVisualizer>();
                if (visualizer == null)
                {
                    visualizer = unitComponent.RootGameObject.AddComponent<UnitDebugVisualizer>();
                }


                if (_attackStash.Has(entity))
                {
                    ref var attackComponent = ref _attackStash.Get(entity);
                    visualizer.AttackRadius = attackComponent.AttackRange;
                }
                else
                {
                    visualizer.AttackRadius = 0f;
                }


                if (_targetStash.Has(entity))
                {
                    ref var targetComponent = ref _targetStash.Get(entity);
                    var hasValidTarget = targetComponent.TargetEntity != default && !World.IsDisposed(targetComponent.TargetEntity);
                    visualizer.HasTarget = hasValidTarget;
                    visualizer.TargetPosition = hasValidTarget ? targetComponent.TargetPosition : unitComponent.RootTransform.position;
                }
                else
                {
                    visualizer.HasTarget = false;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}