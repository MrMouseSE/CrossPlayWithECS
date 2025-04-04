using System.Collections.Generic;

namespace Runtime.Spawning.Utils
{
    public static class UnitStack
    {
        public static List<(int, int)> LevelAndCount = new List<(int, int)>() { (0, 15) };
        
        public static void Add(int level, int count) => LevelAndCount.Add((level, count));

        public static (int level, int count) GetFirstUnit()
        {
            (int level, int count) firstUnit = LevelAndCount[0];
            firstUnit.count--;
            LevelAndCount[0] = firstUnit;
            
            if (firstUnit.count == 0) 
                LevelAndCount.RemoveAt(0);
            
            return firstUnit;
        }
    }
}