using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace TetrisMechanics.Scripts.BlockDestroySystem
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct BlockDestroyComponent : IComponent
    {
        public GameObject BlockGameObject;
        public BlockContainerMono BlockContainer;
        public GameObject BlockVisualObject;
        public ParticleSystem DestroyVFX;
        [HideInInspector]
        public int HorizontalIndex;
        [HideInInspector]
        public int VerticalIndex;
        [HideInInspector]
        public bool ShouldBeDestroyed;

        public bool IsInDestruction;
        private float _timeToDestruction;

        public void Destroy(float deltaTime)
        {
            if (!IsInDestruction)
            {
                _timeToDestruction = DestroyVFX.main.duration;
                DestroyVFX.Play();
                BlockVisualObject.SetActive(false);
                IsInDestruction = true;
            }
            _timeToDestruction -= deltaTime;
            if (_timeToDestruction > 0f) return;
            Object.Destroy(BlockGameObject);
        }
    }
}