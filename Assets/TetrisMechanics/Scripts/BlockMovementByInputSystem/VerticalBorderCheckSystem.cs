using Scellecs.Morpeh;
using TetrisMechanics.Scripts.BlockSpawnSystem;
using TetrisMechanics.Scripts.BlockSystem;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace TetrisMechanics.Scripts.BlockMovementByInputSystem
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class VerticalBorderCheckSystem : ISystem
    {
        public World World { get; set; }

        public void OnAwake()
        {
        }

        public void OnUpdate(float deltaTime)
        {
            Filter currentSelectionFilter = World.Filter.With<CurrentSelectedComponent>().With<BlockMoveComponent>().Build();
            if (currentSelectionFilter.IsEmpty()) return;
            Stash<CurrentSelectedComponent> currentSelectedStash = World.GetStash<CurrentSelectedComponent>();
            Stash<BlockMoveComponent> currentMoveStash = World.GetStash<BlockMoveComponent>();
            Filter spawnFilter = World.Filter.With<SpawnComponent>().Build();
            Stash<SpawnComponent> spawnStash = World.GetStash<SpawnComponent>();
            var filter = World.Filter.With<InputHolderComponent>().Build();
            var inputHolderComponentStash = World.GetStash<InputHolderComponent>();
            ref var inputHolderComponent = ref inputHolderComponentStash.Get(filter.First());

            ref var spawnComponent = ref spawnStash.Get(spawnFilter.First());

            Vector3 offsetVector = Vector3.zero;
            foreach (var entity in currentSelectionFilter)
            {
                ref var currentMoveComponent = ref currentMoveStash.Get(entity);
                if (currentMoveComponent.BlockTransform.position.x + inputHolderComponent.HorizontalMovementDirection.x>
                    spawnComponent.SpawnAnchor.position.x + spawnComponent.SpawnHorizontalOffset)
                {
                    offsetVector = Vector3.left;
                }
                else if (currentMoveComponent.BlockTransform.position.x + inputHolderComponent.HorizontalMovementDirection.x <
                         spawnComponent.SpawnAnchor.position.x)
                {
                    offsetVector = Vector3.right;
                }
            }
            
            inputHolderComponent.UpdateHorizontalMovementDirection(offsetVector);
        }

        public void Dispose()
        {
        }
    }
}