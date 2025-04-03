using UnityEngine;

namespace TetrisMechanics.Scripts
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Create ScoreData", fileName = "ScoreData", order = 0)]
    public class ScoreData : ScriptableObject
    {
        public int ScoreForLideDestroy;
    }
}