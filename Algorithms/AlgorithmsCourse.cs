using Algorithms.Lib.GraphsAlghorithms.Travelling;
using Algorithms.Lib.GraphsAlghorithms.Trees;
using System;

namespace Algorithms
{
    static class AlgorithmsCourse
    {
        public static void Task7()
        {
            //var tree = TreesAlghorithms.CreatePartSortTree(new[] { 5, 3, 8, 1, 4, 9, 22 });
            var tree = PartSortedHeap.CreateHeapTree(new[] { 5, 3, 8, 1, 4, 9, 22 });
        }
        public static void Task10()
        {
            var travelling = new TravellingAlghorithms(Console.WriteLine);
            travelling.Applay(new int[,] {
                                { -1,  68,  73, 24, 70,   9 },
                                { 58,  -1,  16, 44, 11,  92 },
                                { 63,   9,  -1, 86, 13,  18 },
                                { 17,  34,  76, -1, 52,  70 },
                                { 60,  18,   3, 45, -1,  58 },
                                { 16,   82, 11, 60, 48,  -1 },
                                },
                                new[] { "A1", "A2", "A3", "A4", "A5", "A6" });
        }
    }
}
