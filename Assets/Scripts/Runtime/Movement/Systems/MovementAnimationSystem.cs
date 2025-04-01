using Runtime.Movement.Components;
using Runtime.Targeting.Components;
using Runtime.Unit.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Runtime.Movement.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MovementAnimationSystem : ISystem
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        public World World { get; set; }

        private Filter _unitFilter;
        private Filter _playerTargetFilter;
        private Stash<NavMeshAgentComponent> _agentStash;
        private Stash<UnitComponent> _unitStash;
        private Stash<AnimatorComponent> _animatorStash;
        
        private const float MinVelocitySqrMagnitude = 0.0125f;

        public void OnAwake()
        {
            _unitFilter = World.Filter.With<NavMeshAgentComponent>().With<AnimatorComponent>().Build();
            _agentStash = World.GetStash<NavMeshAgentComponent>();
            _animatorStash = World.GetStash<AnimatorComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var unitEntity in _unitFilter)
            {
                ref var agentComponent = ref _agentStash.Get(unitEntity);
                ref var animatorComponent = ref _animatorStash.Get(unitEntity);
                
                var currentSqrVelocity = agentComponent.NavMeshAgent.velocity.sqrMagnitude;
                var isMoving = currentSqrVelocity > MinVelocitySqrMagnitude;
                animatorComponent.RootAnimator.SetBool(IsMoving, isMoving);
            }
        }
        

        public void Dispose() { }
    }
}