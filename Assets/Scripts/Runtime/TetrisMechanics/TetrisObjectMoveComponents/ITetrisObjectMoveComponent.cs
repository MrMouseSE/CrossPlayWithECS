using Runtime.TetrisMechanics.TetrisObject;

namespace Runtime.TetrisMechanics.TetrisObjectMoveComponents
{
    public interface ITetrisObjectMoveComponent
    {
        public void InitializeComponent();
        public void UpdateComponent(float deltaTime, ITetrisObject currentTetrisObject, TetrisObjectMovementCurrentRules rules);
    }
}