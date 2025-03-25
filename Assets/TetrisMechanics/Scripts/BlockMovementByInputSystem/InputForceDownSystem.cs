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
            
            Filter currentSelectionFilter = World.Filter.With<CurrentSelectedComponent>().Build();
            if (currentSelectionFilter.IsEmpty()) return;
            Stash<CurrentSelectedComponent> currentSelectionStash = World.GetStash<CurrentSelectedComponent>();
            Filter landedBlocksFilter = World.Filter.With<LandedCheckComponent>().Without<CurrentSelectedComponent>().Build();
            Stash<LandedCheckComponent> landedCheckStash = World.GetStash<LandedCheckComponent>();

            List<(float, Vector3)> positionDiffForEveryElement = new List<(float, Vector3)>(); 
            foreach (var entity in currentSelectionFilter)
            {
                ref var currentSelectedComponent = ref currentSelectionStash.Get(entity);
                foreach (var landedCheckComponent in currentSelectedComponent.BlockLandedComponents)
                {
                    Vector3 currentPosition = landedCheckComponent.BlockTransform.position;
                    currentPosition.y = 0;
                    foreach (var landedEntity in landedBlocksFilter)
                    {
                        ref var landedComponent = ref landedCheckStash.Get(landedEntity);
                        if (!Mathf.Approximately(landedComponent.BlockTransform.position.x, landedCheckComponent.BlockTransform.position.x)) continue;
                        if (currentPosition.y < (landedComponent.BlockTransform.position.y + 1))
                        {
                            positionDiffForEveryElement.Add((landedCheckComponent.BlockTransform.position.y - landedComponent.BlockTransform.position.y + 1, landedCheckComponent.BlockTransform.position));
                        }
                    }
                }
                currentSelectedComponent.Move(Vector3.down * positionDiffForEveryElement.Min(x=>x.Item1));
                
                ref var thisLandedComponent = ref landedCheckStash.Get(entity);
                thisLandedComponent.SetLandedDirectly(true);
            }
        }
    
        public void Dispose()
        {
        }
    }
}