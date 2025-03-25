using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace TetrisMechanics.Scripts.BlockMovementByInputSystem
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct InputHolderComponent : IComponent
    {
        public Vector3 HorizontalMovementDirection;
        public bool IsForcedDown;
        
        private Quaternion _rotation;

        public void SetBlockRotation(int direction)
        {
            _rotation = Quaternion.Euler(0f, 0f, direction * 90f);
        }

        public Quaternion GetBlockRotation()
        {
            return _rotation;
        }

        public void RestoreHoldedData()
        {
            HorizontalMovementDirection = Vector3.zero;
            IsForcedDown = false;
            _rotation = Quaternion.identity;
        }
    }
}