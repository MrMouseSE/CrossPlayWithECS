using Runtime.TetrisMechanics.TetrisObject;
using UnityEngine;

namespace Runtime.TetrisMechanics.TetrisObjectMoveComponents
{
    public class TetrisObjectHorizontalMoveComponent : ITetrisObjectMoveComponent
    {
        public void InitializeComponent()
        {
        }

        public void UpdateComponent(float deltaTime, ITetrisObject currentTetrisObject,
            TetrisObjectMovementCurrentRules rules)
        {
            Vector3 currentOffset = Vector3.zero;
            if (Input.GetKey("a"))
            {
                currentOffset = Vector3.left;
            }
            else if (Input.GetKey("d"))
            {
                currentOffset = Vector3.right;
            }

            currentTetrisObject.TrySetNewPosition(currentOffset);
        }
    }
}