using TMPro;
using UnityEngine;

namespace TetrisMechanics.Scripts
{
    public static class StaticScoreHolder
    {
        public static TextMeshPro ScoreText;
        
        private static int _currentScore;

        public static void AddScore(int score)
        {
            _currentScore += score;
            ScoreText.text = _currentScore.ToString();
        }

        public static int GetScore()
        {
            return _currentScore;
        }
        
        public static void ResetScore()
        {
            _currentScore = 0;
        }

        public static void SaveScore()
        {
            PlayerPrefs.SetInt("Score", _currentScore);
        }

        public static int LoadAndGetScore()
        {
            return PlayerPrefs.GetInt("Score");
        }
    }
}