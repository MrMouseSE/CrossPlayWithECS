using Runtime.TetrisMechanics.TetrisObject;
using UnityEngine;

namespace Runtime.TetrisMechanics.TetrisObjectMoveComponents
{
    public class TetrisObjectVerticalMoveComponent : ITetrisObjectMoveComponent
    {
        private float _currentMoveDownDelay;

        public void InitializeComponent()
        {
        }

        public void UpdateComponent(float deltaTime, ITetrisObject currentTetrisObject, TetrisObjectMovementCurrentRules rules)
        {
            _currentMoveDownDelay -= deltaTime;
            if (!(_currentMoveDownDelay < 0)) return;
            _currentMoveDownDelay = rules.MoveDownDelay;
            if (!currentTetrisObject.TrySetNewPosition(Vector3.down)) currentTetrisObject.SetObjectLanded();
        }
    }
}