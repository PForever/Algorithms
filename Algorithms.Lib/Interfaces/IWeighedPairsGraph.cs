using System.Collections.Generic;

namespace Algorithms.Lib.Interfaces
{
    public interface IWeighedPairsGraph : IPrintable
    {
        IEnumerable<IWeighedEdge> Edges { get; }
        IEnumerable<IPairNode> GetFree();
        bool IsFree(INode last);
        void RemoveEdge(IWeighedEdge edge);
        void AddEdge(IWeighedEdge edge);
    }
}