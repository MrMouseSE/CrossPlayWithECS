using Scellecs.Morpeh;
using TetrisMechanics.Scripts.BlockSystem;
using Unity.IL2CPP.CompilerServices;

namespace TetrisMechanics.Scripts.BlockMovementByInputSystem
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    
    public sealed class InputMovementSystem : ISystem
    {
        public World World { get; set; }
    
        public void OnAwake()
        {
        }

        public void OnUpdate(float deltaTime)
        {
            Filter currentSelectionFilter = World.Filter.With<CurrentSelectedComponent>().With<BlockMoveComponent>().Build();
            if (currentSelectionFilter.IsEmpty()) return;
            Stash<BlockMoveComponent> currentMoveStash = World.GetStash<BlockMoveComponent>();
            Stash<CurrentSelectedComponent> currentSelectedStash = World.GetStash<CurrentSelectedComponent>();
            Filter inputFilter = World.Filter.With<InputHolderComponent>().Build();
            Stash<InputHolderComponent> inputHolderStash = World.GetStash<InputHolderComponent>();
            

            foreach (var entity in currentSelectionFilter)
            {
                ref var currentMoveComponent = ref currentMoveStash.Get(entity);
                ref var currentSelectedComponent = ref currentSelectedStash.Get(entity);
                ref var holder = ref inputHolderStash.Get(inputFilter.First());
                currentMoveComponent.UpdateBlockPosition(holder.HorizontalMovementDirection);
                currentMoveComponent.UpdateBlockPosition(holder.VerticalMovementDirection);
                currentMoveComponent.UpdatePositionByRotation(currentSelectedComponent.RotateTransform.position, holder.GetBlockRotation());
            }
        }

        public void Dispose()
        {
        }
    }
}