using System.Collections.Generic;

namespace Algorithms.Lib.Interfaces
{
    public interface ITwoPartsWeighedRout : IPrintable
    {
        IEnumerable<IPairNode> NodeXs { get; }
        IEnumerable<IPairNode> NodeYs { get; }
        int NodeCount { get; }
        IEnumerable<IWeighedEdge> Edges { get; }

        void AddEdge(IWeighedEdge edge);
    }
}