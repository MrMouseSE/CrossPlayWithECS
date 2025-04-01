using System.Collections.Generic;
using TetrisMechanics.Scripts.BlockDestroySystem;
using UnityEngine;

namespace TetrisMechanics.Scripts
{
    public static class StaticLinesHolder
    {
        private static BlockContainerMono[][] _blockDestroyComponents = new BlockContainerMono[25][];

        public static void InitLines()
        {
            for (var index = 0; index < _blockDestroyComponents.Length; index++)
            {
                _blockDestroyComponents[index] = new BlockContainerMono[10];
            }
        }

        public static void AddBlockDestroyComponent(BlockDestroyComponent blockDestroyComponent)
        {
            _blockDestroyComponents[blockDestroyComponent.VerticalIndex][blockDestroyComponent.HorizontalIndex] = blockDestroyComponent.BlockContainer;
        }

        public static List<int> GetFullLines()
        {
            List<int> fullLines = new List<int>();
            for (var index = 0; index < _blockDestroyComponents.Length; index++)
            {
                var blockDestroyComponent = _blockDestroyComponents[index];
                int lineElements = 0;
                for (var i = 0; i < blockDestroyComponent.Length; i++)
                {
                    if (blockDestroyComponent[i] != null) lineElements++;
                }

                if (lineElements == 10)
                {
                    fullLines.Add(index);
                }
            }
            return fullLines;
        }

        public static void ClearLine(int index)
        {
            for (var i = 0; i < _blockDestroyComponents[index].Length; i++)
            {
                _blockDestroyComponents[index][i] = null;
            }
        }

        public static bool CheckCellForOccupied(Transform blockTransform)
        {
            int verticalIndex = Mathf.RoundToInt(blockTransform.position.y);
            int horizontalIndex = Mathf.RoundToInt(blockTransform.position.x);
            return _blockDestroyComponents[verticalIndex][horizontalIndex] != null;
        }

        public static bool CheckDownCellForOccupied(Transform blockTransform)
        {
            int verticalIndex = Mathf.RoundToInt(blockTransform.position.y);
            int horizontalIndex = Mathf.RoundToInt(blockTransform.position.x);
            return _blockDestroyComponents[verticalIndex-1][horizontalIndex] != null;
        }

        public static float FindBlockBelowThis(Transform blockTransform)
        {
            int verticalIndex = Mathf.RoundToInt(blockTransform.position.y);
            int horizontalIndex = Mathf.RoundToInt(blockTransform.position.x);
            int verticalOffset = verticalIndex;
            for (int index = 0; index < verticalIndex; index++)
            {
                if (_blockDestroyComponents[index][horizontalIndex] != null && (verticalOffset - index) < verticalOffset)
                {
                    verticalOffset = index;
                }
            }
            return verticalOffset;
        }

        public static void RemoveBlockDestroyComponent(Transform blockTransform)
        {
            int verticalIndex = Mathf.RoundToInt(blockTransform.position.y);
            int horizontalIndex = Mathf.RoundToInt(blockTransform.position.x);
            _blockDestroyComponents[verticalIndex][horizontalIndex] = null;
        }
    }
}
