using Scellecs.Morpeh;
using TetrisMechanics.Scripts.BlockSystem;
using Unity.IL2CPP.CompilerServices;

namespace TetrisMechanics.Scripts.BlockMovementByInputSystem
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InputRotationSystem : ISystem
    {
        public World World { get; set; }

        public void OnAwake()
        {
        }
    
        public void OnUpdate(float deltaTime)
        {
            Filter currentSelectionFilter = World.Filter.With<CurrentSelectedComponent>().Build();
            if (currentSelectionFilter.IsEmpty()) return;
            Stash<CurrentSelectedComponent> currentSelectedStash = World.GetStash<CurrentSelectedComponent>();
            Filter inputFilter = World.Filter.With<InputHolderComponent>().Build();
            Stash<InputHolderComponent> inputHolderStash = World.GetStash<InputHolderComponent>();
            
            foreach (var entity in currentSelectionFilter)
            {
                ref var currentSelectedComponent = ref currentSelectedStash.Get(entity);
                currentSelectedComponent.Rotate(inputHolderStash.Get(inputFilter.First()).GetBlockRotation());
            }
        }
    
        public void Dispose()
        {
        }
    }
}