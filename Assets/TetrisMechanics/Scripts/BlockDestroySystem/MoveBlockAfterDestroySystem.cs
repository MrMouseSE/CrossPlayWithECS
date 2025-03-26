using Scellecs.Morpeh;
using TetrisMechanics.Scripts.BlockSystem;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace TetrisMechanics.Scripts.BlockDestroySystem
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MoveBlockAfterDestroySystem : ISystem
    {
        private Filter _filter;
        private Stash<MoveBlockAfterDestroyComponent> _moveAfterDestroyStash;
        public World World { get; set; }

        public void OnAwake()
        {
        }

        public void OnUpdate(float deltaTime)
        {
            _filter = World.Filter.With<MoveBlockAfterDestroyComponent>().Build();
            _moveAfterDestroyStash = World.GetStash<MoveBlockAfterDestroyComponent>();

            foreach (var entity in _filter)
            {
                ref var destroyComponent = ref _moveAfterDestroyStash.Get(entity);

                destroyComponent.TimeToMove -= deltaTime;
                if (!(destroyComponent.TimeToMove <= 0)) continue;
                var blockMoveComponentFilter = World.Filter.With<BlockMoveComponent>().Build();
                var blockMoveStash = World.GetStash<BlockMoveComponent>();
                var blockDestroyStash = World.GetStash<BlockDestroyComponent>();
                foreach (var entityToMove in blockMoveComponentFilter)
                {
                    ref var blockMoveComponent = ref blockMoveStash.Get(entityToMove);
                    ref var blockDestroyComponent = ref blockDestroyStash.Get(entityToMove);
                    if (blockDestroyComponent.VerticalIndex > destroyComponent.DestroyVerticalIndex)
                    {
                        blockMoveComponent.UpdateBlockPosition(Vector3.down);
                        StaticLinesHolder.AddBlockDestroyComponent(blockDestroyComponent);
                    }
                }
                _moveAfterDestroyStash.Remove(entity);
            }
        }

        public void Dispose()
        {
        }
    }
}