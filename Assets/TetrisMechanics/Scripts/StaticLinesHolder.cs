using System;
using System.Collections.Generic;
using TetrisMechanics.Scripts.BlockDestroySystem;

namespace TetrisMechanics.Scripts
{
    public static class StaticLinesHolder
    {
        private static BlockContainerMono[][] _blockDestroyComponents = new BlockContainerMono[21][];

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
    }
}
