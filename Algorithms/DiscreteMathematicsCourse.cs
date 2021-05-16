using Algorithms.Lib.GraphsAlghorithms;
using Algorithms.Lib.Interfaces;
using System;
using System.Linq;


namespace Algorithms
{
    static class DiscreteMathematicsCourse
    {
        public static void Task7(IGraphAtoFactory graphFactory)
        {
            //var graph = graphFactory.CreateTwoPartsWeighedGraph(new int[,] {
            //                                                { 4,  3,  7,  9,  12, 10 },
            //                                                { 5,  7,  11, 10, 14, 6  },
            //                                                { 3,  8,  12, 11, 4,  9  },
            //                                                { 11, 9,  14, 7,  6,  13 },
            //                                                { 10, 12, 4,  3,  5,  8  },
            //                                                { 12, 4,  3,  8,  7,  12 },
            //                                                },
            //                                                    new[] { "1", "2", "3", "4", "5", "6" },
            //                                                    new[] { "A", "B", "C", "D", "E", "F" });
            var graph = graphFactory.CreateTwoPartsWeighedGraph(new int[,] {
                                                            { 8, 8,  1,  10, 4, 6, 6, 1  },
                                                            { 6, 8,  8,  10, 3, 3, 3, 10 },
                                                            { 3, 4,  10, 2,  3, 3, 3, 1  },
                                                            { 8, 1,  6,  4,  4, 4, 9, 5  },
                                                            { 2, 4,  9,  4,  5, 3, 3, 4  },
                                                            { 1, 5,  4,  7,  2, 9, 3, 8  },
                                                            { 8, 3,  3,  3,  4, 4, 4, 10 },
                                                            { 8, 5,  1,  1,  1, 9, 8, 3  },
                                                            },
                                                                new[] { "1", "2", "3", "4", "5", "6", "7", "8" },
                                                                new[] { "A", "B", "C", "D", "E", "F", "G", "H" });
            void OnStep(string info) => Console.WriteLine(info);
            var pairs = graph.ApplayHungarian(graphFactory, OnStep);
            Console.WriteLine(pairs.Print());
        }

        public static void Task8(IGraphAtoFactory graphFactory)
        {
            var graph = graphFactory.CreateGraph(("H", "E"), ("H", "F"), ("H", "D"), ("H", "G"),
                                                    ("E", "A"), ("E", "D"), ("E", "B"),
                                                    ("F", "A"), ("F", "D"), ("F", "G"),
                                                    ("G", "B"), ("G", "C"),
                                                    ("D", "A"),
                                                    ("A", "B"),
                                                    ("B", "C"));
            Console.WriteLine(graph);
            foreach (var step in graph.ApplayFleury(graphFactory))
            {
                Console.WriteLine(step);
            }
        }
        public static void Task9(IGraphAtoFactory graphFactory)
        {
            //var graph = graphFactory.CreateOrgraph(("F", "A"), ("C", "A"), ("A", "B"),
            //                                       ("F", "B"), ("B", "E"), ("B", "C"),
            //                                       ("F", "C"), ("D", "F"),
            //                                       ("E", "C"), ("C", "D"), ("D", "C"),
            //                                       ("E", "D"));
            var graph = graphFactory.CreateOrgraph(new int[,] {
                                                            { 0, 0, 1, 1 },
                                                            { 0, 0, 1, 1 },
                                                            { 1, 1, 0, 1 },
                                                            { 1, 1, 1, 0 },
                                                            },
                                                                "A", "B", "C", "D");
            Console.WriteLine(graph);
            var result = graph.ApplayRobertsFlores(graphFactory).ToList();
            result.ForEach(Console.WriteLine);
        }
    }
}
