using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace TetrisMechanics.Scripts.BlockSystem
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class BlockMoveSystem : ISystem
    {
        public World World { get; set; }
        
        private Filter _filter;
        private Stash<BlockMoveComponent> _moveBlockStash;
        private Stash<StepAwaiterComponent> _stepAwaiterStash;
        private Stash<LandedCheckComponent> _landingCheckStash;

        public void OnAwake()
        {
        }

        public void OnUpdate(float deltaTime)
        {
            _filter = World.Filter.With<BlockMoveComponent>().With<StepAwaiterComponent>().With<LandedCheckComponent>().Build();
            _moveBlockStash = World.GetStash<BlockMoveComponent>();
            _stepAwaiterStash = World.GetStash<StepAwaiterComponent>();
            _landingCheckStash = World.GetStash<LandedCheckComponent>();
            
            foreach (var entity in _filter)
            {
                ref var moveBlockComponent = ref _moveBlockStash.Get(entity);
                ref var stepAwaiterComponent = ref _stepAwaiterStash.Get(entity);
                ref var landingCheckComponent = ref _landingCheckStash.Get(entity);
                if (!stepAwaiterComponent.UpdateTimeAndCheckReadyForEvaluate(deltaTime)) continue;
                if (!landingCheckComponent.IsLanded) moveBlockComponent.UpdateBlockPosition(Vector3.down);
            }
        }

        public void Dispose()
        {
        }
    }
}