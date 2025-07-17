using System.Collections.Generic;

namespace Runtime.TetrisMechanics.TetrisPartComponents
{
    public class EmptyPartComponent : ITetrisPartComponent
    {
        public void InitializePart()
        {
            throw new System.NotImplementedException();
        }

        public void UpdatePart(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public void DestroyPart(ITetrisPartComponent[,] currentPartsHolder, List<ITetrisPartComponent> emptyPartsHolder, 
            int linePosition, int height)
        {
        }
    }
}