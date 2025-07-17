using System.Collections.Generic;
using Runtime.TetrisMechanics.TetrisObject;

namespace Runtime.TetrisMechanics.TetrisObjectMoveComponents
{
    public class TetrisObjectMoveSystem : ITetrisSystem
    {
        private List<ITetrisObjectMoveComponent> _moveComponents;

        public TetrisObjectMoveSystem()
        {
            _moveComponents = new List<ITetrisObjectMoveComponent>();
            _moveComponents.Add(new TetrisObjectVerticalMoveComponent());
            _moveComponents.Add(new TetrisObjectHorizontalMoveComponent());
            _moveComponents.Add(new TetrisObjectRotationComponent());
        }

        public ITetrisObject UpdateTetrisSystem(float deltaTime, ITetrisObject currentTetrisObject, TetrisObjectMovementCurrentRules rules)
        {
            if (currentTetrisObject.IsObjectLandedCheck()) return currentTetrisObject;
            foreach (var moveComponent in _moveComponents)
            {
                moveComponent.UpdateComponent(deltaTime, currentTetrisObject, rules);
            }
            return currentTetrisObject;
        }
    }
}