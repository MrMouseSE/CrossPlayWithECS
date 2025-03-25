using Scellecs.Morpeh;
using TetrisMechanics.Scripts.BlockMovementByInputSystem;
using TetrisMechanics.Scripts.BlockSystem;
using Unity.IL2CPP.CompilerServices;

namespace TetrisMechanics.Scripts.BlockSpawnSystem
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CurrentSelectedSystem : ISystem
    {
        public World World { get; set; }
        Filter _filter;
        private Stash<CurrentSelectedComponent> _currentSelectedStash;
        private Stash<LandedCheckComponent> _landedStash;

        public void OnAwake()
        {
        }
    
        public void OnUpdate(float deltaTime)
        {
            _filter = World.Filter.With<CurrentSelectedComponent>().Build();
            if (_filter.IsEmpty()) return;
            
            _currentSelectedStash = World.GetStash<CurrentSelectedComponent>();
            _landedStash = World.GetStash<LandedCheckComponent>();

            ref var currentLandedComponent = ref _landedStash.Get(_filter.First());
            if (!currentLandedComponent.IsLanded) return;
            ref var currentSelected = ref _currentSelectedStash.Get(_filter.First());
            currentSelected.Dispose();
        }
    
        public void Dispose()
        {
        }
    }
}