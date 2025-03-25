using System.Collections.Generic;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace TetrisMechanics.Scripts.BlockDestroySystem
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class HorizontalLineFillCheckSystem : ISystem
    {
        public World World { get; set; }
        
        private Filter _filter;
        private Stash<BlockDestroyComponent> _destroyComponentStash;

        public void OnAwake()
        {
        }
    
        public void OnUpdate(float deltaTime)
        {
            _filter = World.Filter.With<BlockDestroyComponent>().Build();
            _destroyComponentStash = World.GetStash<BlockDestroyComponent>();
            
            List<int> fullLines = StaticLinesHolder.GetFullLines();

            foreach (var entity in _filter)
            {
                ref var blockDestroyComponent = ref _destroyComponentStash.Get(entity);
                blockDestroyComponent.ShouldBeDestroyed = fullLines.Contains(blockDestroyComponent.VerticalIndex);
            }

            for (var index = 0; index < fullLines.Count; index++)
            {
                var newMoveBlockAfterDestroyEntity = World.CreateEntity();
                var moveAfterDestroyStash = World.GetStash<MoveBlockAfterDestroyComponent>();
                moveAfterDestroyStash.Set(newMoveBlockAfterDestroyEntity,
                    new MoveBlockAfterDestroyComponent { TimeToMove = 1.2f, DestroyVerticalIndex = index });
                StaticLinesHolder.ClearLine(index);
            }
        }
    
        public void Dispose()
        {
        }
    }
}