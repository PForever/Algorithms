using Algorithms.Lib.GraphsAlghorithms;
using Algorithms.Lib.Implementations.Simple;
using Algorithms.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Unicode;

namespace Algorithms
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            var graphFactory = new GraphAtoFactory();
            Task7(graphFactory);
            Task8(graphFactory);
            //Task9(graphFactory);
        }

        private static void Task7(IGraphAtoFactory graphFactory)
        {
            var graph = graphFactory.CreateWeighedPairGraph(new int[,] {
                                                                { 0, 0, 1, 1 },
                                                                { 0, 0, 1, 1 },
                                                                { 1, 1, 0, 1 },
                                                                { 1, 1, 1, 0 },
                                                               },
                                                               new[] { "1", "2", "3", "4" },
                                                               new[] { "A", "B", "C", "D" });
            void OnStep(string info) => Console.WriteLine(info);
            var pairs = graph.ApplayHungarian(graphFactory, OnStep);
        }

        private static void Task8(IGraphAtoFactory graphFactory)
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
        private static void Task9(IGraphAtoFactory graphFactory)
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
