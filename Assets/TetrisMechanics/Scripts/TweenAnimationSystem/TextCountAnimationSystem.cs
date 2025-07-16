using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace TetrisMechanics.Scripts.TweenAnimationSystem
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]

    public sealed class TextCountAnimationSystem : ISystem
    {
        public World World { get; set; }
        private Filter _filter;
        private Stash<TextCountAnimationComponent> _countAnimationStash;
        
        public void OnAwake()
        {
            _filter = World.Filter.With<TextCountAnimationComponent>().Build();
            _countAnimationStash = World.GetStash<TextCountAnimationComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var countAnimationComponents = ref _countAnimationStash.Get(entity);
                countAnimationComponents.UpdateTextValue(deltaTime);
            }
        }

        public void Dispose()
        {
        }
    }
}