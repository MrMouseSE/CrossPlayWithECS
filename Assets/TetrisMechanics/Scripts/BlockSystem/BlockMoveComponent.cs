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

        public void UpdateBlockPosition()
        {
            BlockTransform.Translate(Vector3.down);
        }
    }
}