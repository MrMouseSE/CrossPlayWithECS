using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace TetrisMechanics.Scripts.TweenAnimationSystem
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]

    public sealed class ScoreValueUpdatedCheckSystem : ISystem
    {
        public World World { get; set; }
        private Filter _filter;
        private Stash<TextCountAnimationComponent> _countAnimationStash;

        private int _currentScore;
        private int _previousScore;
    
        public void OnAwake()
        {
            _filter = World.Filter.With<TextCountAnimationComponent>().Build();
            _countAnimationStash = World.GetStash<TextCountAnimationComponent>();
            foreach (var entity in _filter)
            {
                ref var countAnimationComponents = ref _countAnimationStash.Get(entity);
                countAnimationComponents.Initialize();
            }
        }

        public void OnUpdate(float deltaTime)
        {
            _currentScore = StaticScoreHolder.GetScore();
            if (_currentScore == _previousScore) return;
            foreach (var entity in _filter)
            {
                ref var countAnimationComponents = ref _countAnimationStash.Get(entity);
                countAnimationComponents.ChangeNewTextValue(_previousScore, _currentScore);
            }

            _previousScore = _currentScore;
        }

        public void Dispose()
        {
            // TODO release managed resources here
        }
    }
}