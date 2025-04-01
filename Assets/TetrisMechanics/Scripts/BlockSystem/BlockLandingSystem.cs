using Scellecs.Morpeh;
using TetrisMechanics.Scripts.BlockDestroySystem;
using TetrisMechanics.Scripts.BlockMovementByInputSystem;
using Unity.IL2CPP.CompilerServices;

namespace TetrisMechanics.Scripts.BlockSystem
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class BlockLandingSystem : ISystem
    {
        public World World { get; set; }
        
        private Filter _filter;
        private Stash<LandedCheckComponent> _landedCheckStash;
        private Stash<BlockDestroyComponent> _blockDestroyStash;
        private Stash<BlockDestroyComponent> _blockSelectedStash;

        public void OnAwake()
        {
        }

        public void OnUpdate(float deltaTime)
        {
            _filter = World.Filter.With<LandedCheckComponent>().With<CurrentSelectedComponent>().Build();
            _landedCheckStash = World.GetStash<LandedCheckComponent>();
            _blockDestroyStash = World.GetStash<BlockDestroyComponent>();
            _blockSelectedStash = World.GetStash<BlockDestroyComponent>();

            bool landedNow = false;
            
            foreach (var entity in _filter)
            {
                ref var landingCheckComponent = ref _landedCheckStash.Get(entity);
                if (landingCheckComponent.CheckForLanded() || landingCheckComponent.CheckForLandedOnAnotherBlock())
                {
                    SetLandedDirectly();
                }
            }
        }

        private void SetLandedDirectly()
        {
            foreach (var entity in _filter)
            {
                ref var landingCheckComponent = ref _landedCheckStash.Get(entity);
                landingCheckComponent.SetLandedDirectly(true);
            }
        }

        public void Dispose()
        {
        }
    }
}