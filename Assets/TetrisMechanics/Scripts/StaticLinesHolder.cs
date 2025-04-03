using System.Collections.Generic;
using TetrisMechanics.Scripts.BlockDestroySystem;
using UnityEngine;

namespace TetrisMechanics.Scripts
{
    public static class StaticLinesHolder
    {
        private static BlockContainerMono[,] _blockDestroyComponents = new BlockContainerMono[25,10];

        public static void AddBlockDestroyComponent(BlockDestroyComponent blockDestroyComponent)
        {
            _blockDestroyComponents[blockDestroyComponent.VerticalIndex,blockDestroyComponent.HorizontalIndex] = blockDestroyComponent.BlockContainer;
        }

        public static List<int> GetFullLines()
        {
            List<int> fullLines = new List<int>();
            for (var i = 0; i < _blockDestroyComponents.GetLength(0); i++)
            {
                int lineElements = 0;
                for (var j = 0; j < _blockDestroyComponents.GetLength(1); j++)
                {
                    if (_blockDestroyComponents[i,j] != null) lineElements++;
                }

                if (lineElements == 10)
                {
                    fullLines.Add(i);
                }
            }
            return fullLines;
        }

        public static void ClearLine(int index)
        {
            for (var i = 0; i < _blockDestroyComponents.GetLength(1); i++)
            {
                _blockDestroyComponents[index,i] = null;
            }
        }

        public static bool CheckCellForOccupied(Transform blockTransform)
        {
            int verticalIndex = Mathf.RoundToInt(blockTransform.position.y);
            int horizontalIndex = Mathf.RoundToInt(blockTransform.position.x);
            return _blockDestroyComponents[verticalIndex,horizontalIndex] != null;
        }

        public static bool CheckDownCellForOccupied(Transform blockTransform)
        {
            int verticalIndex = Mathf.RoundToInt(blockTransform.position.y);
            int horizontalIndex = Mathf.RoundToInt(blockTransform.position.x);
            return _blockDestroyComponents[verticalIndex-1,horizontalIndex] != null;
        }

        public static float FindBlockBelowThis(Transform blockTransform)
        {
            int verticalMaxIndex = Mathf.RoundToInt(blockTransform.position.y);
            int horizontalIndex = Mathf.RoundToInt(blockTransform.position.x);
            int verticalOffset = verticalMaxIndex;
            for (int verticalIndex = 0; verticalIndex < verticalMaxIndex; verticalIndex++)
            {
                if (_blockDestroyComponents[verticalIndex,horizontalIndex] != null)
                {
                    verticalOffset -= 1;
                }
            }
            return verticalOffset;
        }

        public static void RemoveBlockDestroyComponent(Transform blockTransform)
        {
            int verticalIndex = Mathf.RoundToInt(blockTransform.position.y);
            int horizontalIndex = Mathf.RoundToInt(blockTransform.position.x);
            _blockDestroyComponents[verticalIndex,horizontalIndex] = null;
        }
    }
}
