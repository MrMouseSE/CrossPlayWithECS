using System.Collections.Generic;
using System.Linq;
using Scellecs.Morpeh;
using TetrisMechanics.Scripts.BlockSystem;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace TetrisMechanics.Scripts.BlockMovementByInputSystem
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InputForceDownSystem : ISystem
    {
        public World World { get; set; }

        public void OnAwake()
        {
        }

        public void OnUpdate(float deltaTime)
        {
            Filter inputFilter = World.Filter.With<InputHolderComponent>().Build();
            Stash<InputHolderComponent> inputHolderStash = World.GetStash<InputHolderComponent>();
            if (!inputHolderStash.Get(inputFilter.First()).IsForcedDown) return;

            Filter currentSelectionFilter =
                World.Filter.With<CurrentSelectedComponent>().With<BlockMoveComponent>().Build();
            if (currentSelectionFilter.IsEmpty()) return;
            Stash<BlockMoveComponent> currentMoveStash = World.GetStash<BlockMoveComponent>();
            
            List<float> positionDiffForEveryElement = new List<float>();
            foreach (var entity in currentSelectionFilter)
            {
                ref var currentMoveComponent = ref currentMoveStash.Get(entity);
                positionDiffForEveryElement.Add(StaticLinesHolder.FindBlockBelowThis(currentMoveComponent.BlockTransform));
            }
            
            var filter = World.Filter.With<InputHolderComponent>().Build();
            var inputHolderComponentStash = World.GetStash<InputHolderComponent>();
            
            ref var inputHolderComponent = ref inputHolderComponentStash.Get(filter.First());
            inputHolderComponent.UpdateVerticalMovementDirection(Vector3.down * positionDiffForEveryElement.Min());
        }

        public void Dispose()
        {
        }
    }
}