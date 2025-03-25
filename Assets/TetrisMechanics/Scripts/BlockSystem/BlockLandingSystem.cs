using Scellecs.Morpeh;
using TetrisMechanics.Scripts.BlockDestroySystem;
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

        public void OnAwake()
        {
        }

        public void OnUpdate(float deltaTime)
        {
            _filter = World.Filter.With<LandedCheckComponent>().Build();
            _landedCheckStash = World.GetStash<LandedCheckComponent>();
            _blockDestroyStash = World.GetStash<BlockDestroyComponent>();
            
            foreach (var entity in _filter)
            {
                ref var landingCheckComponent = ref _landedCheckStash.Get(entity);
                if(landingCheckComponent.CheckForLanded()) return;
                    
                foreach (var blockEntity in _filter)
                {
                    ref var blockComponent = ref _landedCheckStash.Get(blockEntity);
                    if (entity.Id == blockEntity.Id) landingCheckComponent.CheckForLandedOnAnotherBlock(blockComponent);
                }
            }
        }

        public void Dispose()
        {
        }
    }
}