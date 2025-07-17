using System.Collections.Generic;
using Runtime.TetrisMechanics.TetrisObject;
using Runtime.TetrisMechanics.TetrisObjectMoveComponents;

namespace Runtime.TetrisMechanics
{
    public class TetrisController
    {
        private ITetrisObject _currentTetrisObject;
        private TetrisMovementRulesDescription _movementRulesDescription;
        
        private TetrisObjectMovementCurrentRules _tetrisObjectMovementCurrentRules;
        private List<ITetrisSystem> _tetrisSystems;

        public TetrisController(TetrisMovementRulesDescription movementRulesDescription)
        {
            _movementRulesDescription = movementRulesDescription;
            _tetrisSystems = new List<ITetrisSystem>();
            _tetrisSystems.Add(new TetrisObjectSpawnSystem(_tetrisObjectMovementCurrentRules));
            _tetrisSystems.Add(new TetrisObjectMoveSystem());
        }

        public void SetNewMovementRules(TetrisMovementRulesDescription rulesDescription)
        {
            _movementRulesDescription = rulesDescription;
        }

        public void UpdateCurrentMovementRules(int difficulty)
        {
            _tetrisObjectMovementCurrentRules = _movementRulesDescription.Rules[difficulty];
        }

        public void UpdateTetris(float deltaTime)
        {
            foreach (var tetrisSystem in _tetrisSystems)
            {
                _currentTetrisObject = tetrisSystem.UpdateTetrisSystem(deltaTime, _currentTetrisObject, _tetrisObjectMovementCurrentRules);
            }
        }
    }
}
