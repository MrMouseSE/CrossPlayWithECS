using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace TetrisMechanics.Scripts.BlockSystem
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct LandedCheckComponent : IComponent
    {
        public Transform BlockTransform;
        public bool IsLanded;

        public bool CheckForLanded()
        {
            if (BlockTransform.position.y <= 0.1f)
            {
                IsLanded = true;
            }
            return IsLanded;
        }
        
        public bool CheckForLandedOnAnotherBlock(LandedCheckComponent anotherComponent)
        {
            if(!anotherComponent.IsLanded) return IsLanded;
            IsLanded |= Mathf.Approximately(anotherComponent.BlockTransform.position.x, BlockTransform.position.x)
             && Mathf.Approximately(anotherComponent.BlockTransform.position.y, BlockTransform.position.y-1);
            
            return IsLanded;
        }

        public void SetLandedDirectly(bool value)
        {
            IsLanded = value;
        }
    }
}