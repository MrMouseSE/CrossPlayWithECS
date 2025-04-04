using Runtime.Movement.Components;
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
    public sealed class TargetAnimationSystem : ISystem
    {
        public World World { get; set; }

        private Filter _unitFilter;

        private Stash<UnitComponent> _unitStash;
        private Stash<TargetComponent> _targetStash;
        private Stash<AnimatorComponent> _animatorComponentStash;

        private const float SmoothSpeed = 8.0f;
        private const float MinDistanceSqr = 0.001f;

        public void OnAwake()
        {
            _unitFilter = World.Filter.With<UnitComponent>().With<TargetComponent>().With<AnimatorComponent>().Build();

            _unitStash = World.GetStash<UnitComponent>();
            _targetStash = World.GetStash<TargetComponent>();
            _animatorComponentStash = World.GetStash<AnimatorComponent>();
        
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var unitEntity in _unitFilter)
            {
                ref var targetComponent = ref _targetStash.Get(unitEntity);

                if (targetComponent.TargetEntity != default && !World.IsDisposed(targetComponent.TargetEntity))
                {
                    ref var unitComponentTarget = ref _unitStash.Get(targetComponent.TargetEntity); 
                    ref var animatorComponentSelf = ref _animatorComponentStash.Get(unitEntity);  
                    
                    var relativePos = unitComponentTarget.RootTransform.position - animatorComponentSelf.rootSpineTransform.position;
                    var rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                    animatorComponentSelf.rootSpineTransform.rotation = Quaternion.Slerp(animatorComponentSelf.rootSpineTransform.rotation, rotation, SmoothSpeed * deltaTime);
                }
            }
        }

        public void Dispose() { }
    }
}