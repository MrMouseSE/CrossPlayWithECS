using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace TetrisMechanics.Scripts.BlockDestroySystem
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class LandedBlockDestroySystem : ISystem
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

            foreach (var entity in _filter)
            {
                ref var destroyComponent = ref _destroyComponentStash.Get(entity);
                if (destroyComponent.ShouldBeDestroyed || destroyComponent.IsInDestruction)
                {
                    destroyComponent.Destroy(deltaTime);
                }
            }
        }
        
        public void Dispose()
        {
        }
    }
}