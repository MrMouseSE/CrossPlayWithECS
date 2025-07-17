using System;

namespace Runtime.TetrisMechanics.TetrisObjectMoveComponents
{
    [Serializable]
    public class TetrisObjectMovementCurrentRules
    {
        public int Difficulty;
        public float MoveDownDelay;
        public float SpawnNewObjectDelay;
    }
}