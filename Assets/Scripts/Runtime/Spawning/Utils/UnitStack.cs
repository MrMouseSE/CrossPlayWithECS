using System.Collections.Generic;

namespace Runtime.Spawning.Utils
{
    public static class UnitStack
    {
        public static List<(string, int)> IdAndCount = new() { ("UnitPlayerCommon", 3) };

        public static void Add(string id, int count) => IdAndCount.Add((id, count));

        public static (string Id, int count) GetFirstUnit()
        {
            (string level, int count) firstUnit = IdAndCount[0];
            firstUnit.count--;
            IdAndCount[0] = firstUnit;
            
            if (firstUnit.count == 0) 
                IdAndCount.RemoveAt(0);
            
            return firstUnit;
        }
    }
}