using Scellecs.Morpeh;
using TetrisMechanics.Scripts.BlockMovementByInputSystem;
using Unity.IL2CPP.CompilerServices;

namespace TetrisMechanics.Scripts.BlockSpawnSystem
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]

    public sealed class SelectionInitializeSystem : ISystem
    {
        public World World { get; set; }
        
        public void OnAwake()
        {
        }

        public void OnUpdate(float deltaTime)
        {
            Filter currentSelectedFilter = World.Filter.With<CurrentSelectedComponent>().Build();
            if (!currentSelectedFilter.IsNotEmpty()) return;
            Stash<CurrentSelectedComponent> selectedComponentStash = World.GetStash<CurrentSelectedComponent>();
            ref var selectedComponent = ref selectedComponentStash.Get(currentSelectedFilter.First());
            selectedComponent.Initialize();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}