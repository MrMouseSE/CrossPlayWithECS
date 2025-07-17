using UnityEngine;

namespace Runtime.TetrisMechanics.TetrisObject
{
    public interface ITetrisObject
    {
        public void Initialize();
        public void UpdateTetrisObject(float deltaTime);
        public bool TrySetNewPosition(Vector3 offset);
        public bool TrySetNewRotation(float angle);
        public void SetObjectLanded();
        public bool IsObjectLandedCheck();
        public void DestroyTetrisObject();
    }
}