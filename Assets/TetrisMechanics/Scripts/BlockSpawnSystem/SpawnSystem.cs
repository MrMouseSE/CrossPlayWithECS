using Scellecs.Morpeh;
using TetrisMechanics.Scripts.BlockDestroySystem;
using TetrisMechanics.Scripts.BlockMovementByInputSystem;
using TetrisMechanics.Scripts.BlockSystem;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace TetrisMechanics.Scripts.BlockSpawnSystem
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    
    public sealed class SpawnSystem : ISystem
    {
        public World World { get; set; }
        private Filter _filter;
        private Stash<SpawnComponent> _spawnStash;
        private Stash<StepAwaiterComponent> _awaiterStash;

        public void OnAwake()
        {
            _filter = World.Filter.With<SpawnComponent>().With<StepAwaiterComponent>().Build();
            _spawnStash = World.GetStash<SpawnComponent>();
            _awaiterStash = World.GetStash<StepAwaiterComponent>();
        }
    
        public void OnUpdate(float deltaTime)
        {
            Filter currentSelectedFilter = World.Filter.With<CurrentSelectedComponent>().Build();
            if (currentSelectedFilter.IsNotEmpty()) return;
            
            ref var awaiter = ref _awaiterStash.Get(_filter.First());
            if (!awaiter.UpdateTimeAndCheckReadyForEvaluate(deltaTime)) return;
            ref var spawnComponent = ref _spawnStash.Get(_filter.First());
            spawnComponent.SpawnPrefab();
        }
    
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}