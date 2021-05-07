using Algorithms.Lib;
using Algorithms.Lib.Implementations.Simple;
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
            //Console.InputEncoding = Encoding.Unicode;
            var graphFactory = new GraphAtoFactory();
            //var graph = graphFactory.CreateGraph(("1", "2"), ("2", "3"), ("1", "3"), ("1", "4"), ("3", "4"), ("1", "6"), ("6", "4"), ("3", "5"), ("4", "5"));
            //var graph = graphFactory.CreateGraph(("H", "E"), ("H", "F"), ("H", "D"), ("H", "G"),
            //                                     ("E", "A"), ("E", "D"), ("E", "B"),
            //                                     ("F", "A"), ("F", "D"), ("F", "G"),
            //                                     ("G", "B"), ("G", "C"),
            //                                     ("D", "A"),
            //                                     ("A", "B"),
            //                                     ("B", "C"));
            //Console.WriteLine(graph);
            //foreach(var step in graph.ApplayFleury())
            //{
            //    Console.WriteLine(step);
            //}
            var graph = graphFactory.CreateOrgraph(("F", "A"), ("C", "A"), ("A", "B"),
                                                   ("F", "B"), ("B", "E"), ("B", "C"),
                                                   ("F", "C"), ("D", "F"),
                                                   ("E", "C"), ("C", "D"), ("D", "C"),
                                                   ("E", "D"));
            var result = graph.ApplayRobertsFlores().ToList();
            result.ForEach(Console.WriteLine);
            //Console.WriteLine(result);
        }
    }
}
