using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace TetrisMechanics.Scripts.BlockDestroySystem
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct MoveBlockAfterDestroyComponent : IComponent
    {
        public float TimeToMove;
        public int DestroyVerticalIndex;
        public int VerticalOffset;
    }
}