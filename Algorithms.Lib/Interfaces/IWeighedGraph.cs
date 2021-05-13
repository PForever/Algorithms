using System.Collections.Generic;

namespace Algorithms.Lib.Interfaces
{
    public interface IWeighedGraph : IWeighedOrgraph, IGraph
    {
        IReadOnlyCollection<IWeighedEdge> WeighedEdges { get; }
        void RemoveEdge(IWeighedEdge edge);
    }
    public interface IWeighedPairGraph : IPrintable
    {
        IReadOnlyCollection<INode> XNodes { get; }
        IReadOnlyCollection<INode> YNodes { get; }
        IReadOnlyCollection<IWeighedEdge> GetEdges(INode node);
        void DecreaseWeight(INode node, int d);
    }
}