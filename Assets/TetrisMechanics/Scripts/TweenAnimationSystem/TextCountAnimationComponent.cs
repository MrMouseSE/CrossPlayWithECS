using Scellecs.Morpeh;
using TMPro;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace TetrisMechanics.Scripts.TweenAnimationSystem
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    
    public struct TextCountAnimationComponent : IComponent
    {
        public TextMeshPro TextComponent;
        public float AnimationLength;
        public float MaximumAnimationLength;
        public float AnimationStopTimeLenght;
        public AnimationCurve AnimationBehaviour;
        public Color TextNormalColor;
        public Color TextAnimatedColor;
        public float TextNormalSize;
        public float TextAnimatedSize;

        private float _evaluateValue;

        private float _previousTextValue;
        private float _currentTextValue;
        private float _newTextValue;
        private float _currentAnimationTime;
        private float _currentAnimationLenght;
        private float _currentStopTime;
        private bool _isPlaying;

        public void Initialize()
        {
            _currentTextValue = StaticScoreHolder.GetScore();
            TextComponent.text = _currentTextValue.ToString("F0");
        }

        public void ChangeNewTextValue(int previousScore, int currentScore)
        {
            if (!_isPlaying)
            {
                _isPlaying = true;
                _previousTextValue = previousScore;
                _currentAnimationTime = 0f;
            }
            
            _currentAnimationLenght = Mathf.Clamp(_currentAnimationLenght + AnimationLength, 0 , MaximumAnimationLength);
            _newTextValue = currentScore;
        }

        public void UpdateTextValue(float deltaTime)
        {
            if (!_isPlaying) return;
            _evaluateValue = Mathf.Min(_currentAnimationTime, _currentAnimationLenght - _currentAnimationTime)/AnimationStopTimeLenght;
            var evaluatedValue = AnimationBehaviour.Evaluate(_evaluateValue);
            TextComponent.color = Color.Lerp(TextNormalColor, TextAnimatedColor, evaluatedValue);
            TextComponent.fontSize = Mathf.Lerp(TextNormalSize, TextAnimatedSize, evaluatedValue);
            _currentAnimationTime += deltaTime;
            _isPlaying = _currentAnimationTime < _currentAnimationLenght;
            if(!_isPlaying) _currentAnimationTime = _currentAnimationLenght;
            _currentTextValue = Mathf.Lerp(_previousTextValue, _newTextValue, _currentAnimationTime / _currentAnimationLenght);
            TextComponent.text = _currentTextValue.ToString("F0");
        }
    }
}