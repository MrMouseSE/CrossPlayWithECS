using Runtime.TetrisMechanics.TetrisObject;
using Runtime.TetrisMechanics.TetrisObjectMoveComponents;

namespace Runtime.TetrisMechanics
{
    public class TetrisObjectSpawnSystem : ITetrisSystem
    {
        private float _currentSpawnNewObjectDelay; 
        public TetrisObjectSpawnSystem(TetrisObjectMovementCurrentRules rules)
        {
            _currentSpawnNewObjectDelay = rules.SpawnNewObjectDelay;
        }

        public ITetrisObject UpdateTetrisSystem(float deltaTime, ITetrisObject currentTetrisObject, TetrisObjectMovementCurrentRules rules)
        {
            if (!currentTetrisObject.IsObjectLandedCheck()) return currentTetrisObject;
            _currentSpawnNewObjectDelay -= deltaTime;
            if (_currentSpawnNewObjectDelay>0) return currentTetrisObject;
            return TetrisObjectStaticFactory.GetNewTetrisObject();
        }
    }
}