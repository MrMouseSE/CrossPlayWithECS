using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace TetrisMechanics.Scripts.BlockMovementByInputSystem
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct InputAwaiterComponent : IComponent
    {
        public float InputDelay;

        private float _currentDelay;

        public bool CheckDelayIsEnded()
        {
            if (_currentDelay < InputDelay) return false;
            _currentDelay -= InputDelay;
            return true;
        }
    }
}