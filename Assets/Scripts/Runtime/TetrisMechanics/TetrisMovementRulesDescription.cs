using Runtime.TetrisMechanics.TetrisObjectMoveComponents;
using UnityEngine;

namespace Runtime.TetrisMechanics
{
    [CreateAssetMenu(menuName = "Create TetrisMovementRulesDescription", fileName = "TetrisMovementRulesDescription", order = 0)]
    public class TetrisMovementRulesDescription : ScriptableObject
    {
        public TetrisObjectMovementCurrentRules[] Rules;
    }
}