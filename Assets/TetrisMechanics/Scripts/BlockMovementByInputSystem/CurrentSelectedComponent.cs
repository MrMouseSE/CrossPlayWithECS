using System;
using Scellecs.Morpeh;
using TetrisMechanics.Scripts.BlockSystem;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TetrisMechanics.Scripts.BlockMovementByInputSystem
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct CurrentSelectedComponent : IComponent, IDisposable
    {
        public LandedCheckProvider[] LandedProviders;
        [HideInInspector]
        public LandedCheckComponent[] BlockLandedComponents;
        public Transform RotateTransform;
        public CurrentSelectedProvider Provider;

        public void Initialize()
        {
            BlockLandedComponents = new LandedCheckComponent[LandedProviders.Length];
            for (var index = 0; index < LandedProviders.Length; index++)
            {
                var landedProvider = LandedProviders[index];
                BlockLandedComponents[index] =
                    World.Default.GetStash<LandedCheckComponent>().Get(landedProvider.Entity);
            }
        }

        public void Move(Vector3 position)
        {
            foreach (var blockLandedProvider in BlockLandedComponents)
            {
                blockLandedProvider.BlockTransform.position += position;
            }
        }

        public void Rotate(Quaternion rotation)
        {
            foreach (var blockLandedProvider in BlockLandedComponents)
            {
                blockLandedProvider.BlockTransform.position = rotation * (blockLandedProvider.BlockTransform.position - RotateTransform.position);
            }
        }
        
        public void Dispose()
        {
            Object.Destroy(Provider);
        }
    }
}