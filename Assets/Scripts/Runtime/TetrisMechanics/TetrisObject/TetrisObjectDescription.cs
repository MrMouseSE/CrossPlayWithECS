using UnityEngine;

namespace Runtime.TetrisMechanics.TetrisObject
{
    [CreateAssetMenu(menuName = "Create TetrisObjectDescription", fileName = "TetrisObjectDescription", order = 0)]
    public class TetrisObjectDescription : ScriptableObject
    {
        public TetrisObjectBlueprint[] Blueprints;
    }
}