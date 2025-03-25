using Scellecs.Morpeh;
using TetrisMechanics.Scripts.BlockSpawnSystem;
using TetrisMechanics.Scripts.BlockSystem;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace TetrisMechanics.Scripts.BlockMovementByInputSystem
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class VerticalBorderCheckSystem : ISystem
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
            Filter spawnFilter = World.Filter.With<SpawnComponent>().Build();
            Stash<SpawnComponent> spawnStash = World.GetStash<SpawnComponent>();

            ref var spawnComponent = ref spawnStash.Get(spawnFilter.First());

            foreach (var entity in currentSelectionFilter)
            {
                ref var currentSelectedComponent = ref currentSelectedStash.Get(entity);
                Vector3 offsetVector = Vector3.zero;
                foreach (var blockLandedProvider in currentSelectedComponent.BlockLandedComponents)
                {
                    if (blockLandedProvider.BlockTransform.position.x > spawnComponent.SpawnAnchor.position.x + spawnComponent.SpawnHorizontalOffset)
                    {
                        offsetVector = Vector3.left;
                    }
                    else if (blockLandedProvider.BlockTransform.position.x < spawnComponent.SpawnAnchor.position.x)
                    {
                        offsetVector = Vector3.right;
                    }
                }
                currentSelectedComponent.Move(offsetVector);
            }
        }
        
        public void Dispose()
        {
        }
    }
}