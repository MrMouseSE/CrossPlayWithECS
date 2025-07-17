namespace Runtime.TetrisMechanics.TetrisPartComponents
{
    public interface ITetrisPartComponent
    {
        public void InitializePart();
        public void UpdatePart(float deltaTime);
        public void DestroyPart();
    }
}