using System.Collections.Generic;
using Runtime.TetrisMechanics.TetrisPartComponents;
using UnityEngine;

namespace Runtime.TetrisMechanics.TetrisCup
{
    public class TetrisCupHolder
    {
        private ITetrisPartComponent[,] _currentPartsHolder = new ITetrisPartComponent[10, 20];
        private List<ITetrisPartComponent> _emptyPartsHolder = new List<ITetrisPartComponent>();

        public void InitializeCupHolder()
        {
            for (int i = 0; i < 200; i++)
            {
                _emptyPartsHolder.Add(new EmptyPartComponent());
            }

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    _currentPartsHolder[i, j] = _emptyPartsHolder[^1];
                    _emptyPartsHolder.RemoveAt(_emptyPartsHolder.Count - 1);
                }
            }
        }

        public void AddPart(ITetrisPartComponent part, Vector2Int position)
        {
            _currentPartsHolder[position.x, position.y] = part;
        }

        public void DestroyPartsOnHeight(int height)
        {
            for (int i = 0; i < 10; i++)
            {
                _currentPartsHolder[i % 10, i].DestroyPart(_currentPartsHolder, _emptyPartsHolder, i, height);
            }
        }
    }
}