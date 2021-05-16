using System.Collections.Generic;

namespace Algorithms.Lib.Interfaces
{
    public interface ITwoPartsWeighedGraph : IPrintable
    {
        IEnumerable<IPairNode> NodeXs { get; }
        IEnumerable<IPairNode> NodeYs { get; }
        IEnumerable<IPairNode> Nodes { get; }
        IWeighedEdge GetEdge(IPairNode nodeX, IPairNode nodeY);
        IEnumerable<IWeighedEdge> GetEdges(IPairNode node);
        void AddEdge(IWeighedEdge edge);
    }
}