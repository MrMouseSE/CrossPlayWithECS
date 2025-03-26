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
            Stash<CurrentSelectedComponent> currentSelectionStash = World.GetStash<CurrentSelectedComponent>();
            Stash<BlockMoveComponent> currentMoveStash = World.GetStash<BlockMoveComponent>();
            Filter landedBlocksFilter =
                World.Filter.With<LandedCheckComponent>().Without<CurrentSelectedComponent>().Build();
            Stash<LandedCheckComponent> landedCheckStash = World.GetStash<LandedCheckComponent>();

            List<(float, Vector3)> positionDiffForEveryElement = new List<(float, Vector3)>();
            float forceLandedOffset = float.MaxValue;
            foreach (var entity in currentSelectionFilter)
            {
                ref var currentSelectedComponent = ref currentSelectionStash.Get(entity);
                ref var currentMoveComponent = ref currentMoveStash.Get(entity);

                Vector3 currentPosition = currentMoveComponent.BlockTransform.position;
                if (forceLandedOffset > currentPosition.y) forceLandedOffset = currentPosition.y;
                currentPosition.y = 0;
                foreach (var landedEntity in landedBlocksFilter)
                {
                    ref var landedComponent = ref landedCheckStash.Get(landedEntity);
                    if (!Mathf.Approximately(landedComponent.BlockTransform.position.x,
                            currentMoveComponent.BlockTransform.position.x)) continue;
                    if (currentPosition.y < (landedComponent.BlockTransform.position.y + 1))
                    {
                        positionDiffForEveryElement.Add((currentMoveComponent.BlockTransform.position.y - 
                                                         landedComponent.BlockTransform.position.y - 1, 
                            currentMoveComponent.BlockTransform.position));
                    }
                }
                ref var thisLandedComponent = ref landedCheckStash.Get(entity);
                thisLandedComponent.SetLandedDirectly(true);
            }

            if (positionDiffForEveryElement.Count>0)
            {
                forceLandedOffset = Mathf.Min(forceLandedOffset, positionDiffForEveryElement.Min(x => x.Item1));
            }
            
            var filter = World.Filter.With<InputHolderComponent>().Build();
            var inputHolderComponentStash = World.GetStash<InputHolderComponent>();
            
            ref var inputHolderComponent = ref inputHolderComponentStash.Get(filter.First());
            inputHolderComponent.UpdateVerticalMovementDirection(Vector3.down * forceLandedOffset);
        }

        public void Dispose()
        {
        }
    }
}