using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Movement.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AnimatorComponent: IComponent
    {
        public Animator RootAnimator;
        [FormerlySerializedAs("TargetTransform")] public Transform rootSpineTransform;
    }
}