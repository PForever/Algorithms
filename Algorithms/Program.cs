using Algorithms.Lib.Implementations.Simple;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Unicode;

namespace Algorithms
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            var graphFactory = new GraphAtoFactory();
            //AlgorithmsCourse.Task7();
            AlgorithmsCourse.Task10();
            //DiscreteMathematicsCourse.Task7(graphFactory);
            //DiscreteMathematicsCourse.Task8(graphFactory);
            //DiscreteMathematicsCourse.Task9(graphFactory);
        }
    }
}
