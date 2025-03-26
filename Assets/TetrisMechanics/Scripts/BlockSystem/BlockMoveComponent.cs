using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace TetrisMechanics.Scripts.BlockSystem
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct BlockMoveComponent : IComponent
    {
        public Transform BlockTransform;

        public void UpdateBlockPosition(Vector3 offset)
        {
            BlockTransform.Translate(offset);
        }

        public void UpdatePositionByRotation(Vector3 rotateRootPosition, Quaternion rotation)
        {
            BlockTransform.position = rotateRootPosition + (rotation * (BlockTransform.position - rotateRootPosition));
        }
    }
}