using System;
using Scellecs.Morpeh;
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
        public Transform RotateTransform;
        public CurrentSelectedProvider Provider;
        
        public void Dispose()
        {
            Object.Destroy(Provider);
        }
    }
}