using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace TetrisMechanics.Scripts.BlockSystem
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct StepAwaiterComponent : IComponent
    {
        public float StepTime;
        
        private float _currentStepTimeCounter;

        public float ReturnCurrentTime()
        {
            return _currentStepTimeCounter;
        }

        public bool UpdateTimeAndCheckReadyForEvaluate(float deltaTime)
        {
            _currentStepTimeCounter += deltaTime;
            if (_currentStepTimeCounter < StepTime) return false;
            _currentStepTimeCounter = 0;
            return true;
        }
    }
}