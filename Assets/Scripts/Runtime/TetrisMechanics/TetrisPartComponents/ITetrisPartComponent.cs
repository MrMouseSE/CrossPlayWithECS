using System.Collections.Generic;

namespace Runtime.TetrisMechanics.TetrisPartComponents
{
    public interface ITetrisPartComponent
    {
        public void InitializePart();
        public void UpdatePart(float deltaTime);
        public void DestroyPart(ITetrisPartComponent[,] currentPartsHolder, List<ITetrisPartComponent> emptyPartsHolder, 
            int linePosition, int height);
    }
}