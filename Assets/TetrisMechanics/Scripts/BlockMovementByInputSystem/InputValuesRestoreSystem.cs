using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace TetrisMechanics.Scripts.BlockMovementByInputSystem
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]

    public sealed class InputValuesRestoreSystem : ISystem
    {
        public World World { get; set; }
        
        private Filter _filter;
        private Stash<InputHolderComponent> _inputHodlerComponentStash;

        public void OnAwake()
        {
            _filter = World.Filter.With<InputHolderComponent>().Build();
            _inputHodlerComponentStash = World.GetStash<InputHolderComponent>();
        }
        
        public void OnUpdate(float deltaTime)
        {
            ref var inputHolderComponent = ref _inputHodlerComponentStash.Get(_filter.First());
            inputHolderComponent.RestoreHoldedData();
        }

        public void Dispose()
        {
        }
    }
}