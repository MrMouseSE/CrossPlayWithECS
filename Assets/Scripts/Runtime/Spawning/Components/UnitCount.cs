using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.Serialization;

namespace Runtime.Spawning.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct UnitCount : IComponent
    {
        public int Value;
    }
}