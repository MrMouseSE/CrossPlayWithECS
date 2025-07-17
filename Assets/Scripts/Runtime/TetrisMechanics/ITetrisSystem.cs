using Runtime.TetrisMechanics.TetrisObject;
using Runtime.TetrisMechanics.TetrisObjectMoveComponents;

namespace Runtime.TetrisMechanics
{
    public interface ITetrisSystem
    {
        public ITetrisObject UpdateTetrisSystem(float deltaTime, ITetrisObject currentTetrisObject, TetrisObjectMovementCurrentRules rules);
    }
}
