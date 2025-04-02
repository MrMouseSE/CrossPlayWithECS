using Scellecs.Morpeh;
using TetrisMechanics.Scripts.BlockDestroySystem;
using TetrisMechanics.Scripts.BlockMovementByInputSystem;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace TetrisMechanics.Scripts.BlockSystem
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class LandedBlockSetToHolderSystem : ISystem
    {
        private Filter _filter;
        private Stash<LandedCheckComponent> _landedStash;
        private Stash<BlockDestroyComponent> _destroyStash;
        public World World { get; set; }

        public void OnAwake()
        {
        }
    
        public void OnUpdate(float deltaTime)
        {
            _filter = World.Filter.With<LandedCheckComponent>().With<CurrentSelectedComponent>().Build();
            _landedStash = World.GetStash<LandedCheckComponent>();
            _destroyStash = World.GetStash<BlockDestroyComponent>();

            foreach (var entity in _filter)
            {
                ref var landedComponent = ref _landedStash.Get(entity);
                ref var blockDestroyComponent = ref _destroyStash.Get(entity);
                if (!landedComponent.IsLanded) continue;
                if (blockDestroyComponent.IsInDestruction) return;
                blockDestroyComponent.VerticalIndex = Mathf.FloorToInt(landedComponent.BlockTransform.position.y);
                blockDestroyComponent.HorizontalIndex = Mathf.FloorToInt(landedComponent.BlockTransform.position.x);
                StaticLinesHolder.AddBlockDestroyComponent(blockDestroyComponent);
            }
        }
    
        public void Dispose()
        {
        }
    }
}