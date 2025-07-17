using Runtime.TetrisMechanics.TetrisObject;
using UnityEngine;

namespace Runtime.TetrisMechanics.TetrisObjectMoveComponents
{
    public class TetrisObjectRotationComponent : ITetrisObjectMoveComponent
    {
        public void InitializeComponent()
        {
        }

        public void UpdateComponent(float deltaTime, ITetrisObject currentTetrisObject, TetrisObjectMovementCurrentRules rules)
        {
            float rotationAngle = 0;
            if (Input.GetKey("w"))
            {
                rotationAngle = 90;
            }
            else if (Input.GetKey("s"))
            {
                rotationAngle = -90;
            }

            currentTetrisObject.TrySetNewRotation(rotationAngle);
        }
    }
}